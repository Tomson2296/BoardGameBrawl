#nullable disable
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using BoardruleBrawl.Data.Stores.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.Boardgame
{
    public class ModifyBoardgameModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;
        private readonly IBoardgameRuleStore<BoardgameRule, BoardgameModel, MatchmakingRule> _boardgameRuleStore;
        private readonly IMatchmakingRuleStore<MatchmakingRule> _matchmakingRuleStore;

        public ModifyBoardgameModel(UserManager<ApplicationUser> userManager,
            IBoardGameStore<BoardgameModel> boardgameStore,
            IBoardgameRuleStore<BoardgameRule, BoardgameModel, MatchmakingRule> boardgameRuleStore,
            IMatchmakingRuleStore<MatchmakingRule> matchmakingRuleStore)
        {
            _userManager = userManager;
            _boardgameStore = boardgameStore;
            _boardgameRuleStore = boardgameRuleStore;
            _matchmakingRuleStore = matchmakingRuleStore;
        }

        [BindProperty(SupportsGet = true)]
        public int BoardgameID { get; set; }

        public BoardgameModel Boardgame { get; set; }

        [BindProperty]
        public IEnumerable<MatchmakingRuleDTO> MatchmakingRules { get; set; } = new List<MatchmakingRuleDTO>();

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public RuleType RuleTypeSelect { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel()
        {
            [Display(Name = "RuleDescription")]
            [Required]
            [MaxLength(100)]
            public string RuleDescription { get; set; }

            [Display(Name = "Rule Decider")]
            [Required]
            public bool RuleDecider { get; set; }

            [Required]
            public RuleType RuleType { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(BoardgameID);

            if (await _boardgameRuleStore.CheckIfBGHasAnyMatchmakingRulesAsync(Boardgame.Id))
            {
                MatchmakingRules = await _boardgameRuleStore.GetBoardgameMatchmakingRulesListAsync(Boardgame.Id);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddRuleToBoardgame()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    BoardgameModel boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(BoardgameID);

                    // create a matchmakingRule object and save it to database context
                    MatchmakingRule matchmakingRule = CreateMatchmakingRuleModel();
                    await _matchmakingRuleStore.SetRuleDescriptionAsync(matchmakingRule, Input.RuleDescription);
                    await _matchmakingRuleStore.SetRuleDeciderAsync(matchmakingRule, Input.RuleDecider);
                    await _matchmakingRuleStore.SetRuleTypeAsync(matchmakingRule, RuleTypeSelect);

                    IdentityResult matchmakingRuleCreated = await _matchmakingRuleStore.CreateRuleAsync(matchmakingRule);
                    if (matchmakingRuleCreated.Succeeded)
                    {
                        // create a new boardgameRule object to make a connection in database between newly created matchmaking rule and boardgame
                        BoardgameRule boardgameRule = CreateBoardgameRuleModel();
                        await _boardgameRuleStore.SetMatchmakingRuleIdAsync(boardgameRule, matchmakingRule);
                        await _boardgameRuleStore.SetBoardgameIdAsync(boardgameRule, boardgame);

                        IdentityResult boardgameRuleCreated = await _boardgameRuleStore.CreateBoardgameRuleAsync(boardgameRule, CancellationToken.None);
                        if (boardgameRuleCreated.Succeeded)
                        {
                            return RedirectToPage();
                        }
                        else
                        {
                            StatusMessage = "Error during adding a boardgaming rule to database. Try again later";
                            return Page();
                        }
                    }
                    else
                    {
                        StatusMessage = "Error during adding a matchmaking rule to database. Try again later";
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error during adding a matchmaking rule to database - {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }


        private MatchmakingRule CreateMatchmakingRuleModel()
        {
            try
            {
                return Activator.CreateInstance<MatchmakingRule>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(MatchmakingRule)}'.");
            }
        }

        private BoardgameRule CreateBoardgameRuleModel()
        {
            try
            {
                return Activator.CreateInstance<BoardgameRule>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BoardgameRule)}'.");
            }
        }
    }
}