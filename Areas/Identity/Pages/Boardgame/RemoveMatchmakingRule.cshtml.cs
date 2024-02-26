#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Data;

namespace BoardGameBrawl.Areas.Identity.Pages.Boardgame
{
    public class RemoveMatchmakingRuleModel : PageModel
    { 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;
        private readonly IMatchmakingRuleStore<MatchmakingRule> _matchmakingRuleStore;
        private readonly IBoardgameRuleStore<BoardgameRule, BoardgameModel, MatchmakingRule> _boardgameRuleStore;

        public RemoveMatchmakingRuleModel(UserManager<ApplicationUser> userManager,
             IBoardGameStore<BoardgameModel> boardgameStore,
             IMatchmakingRuleStore<MatchmakingRule> matchmakingRuleStore,
             IBoardgameRuleStore<BoardgameRule, BoardgameModel, MatchmakingRule> boardgameRuleStore)
        {
            _userManager = userManager;
            _boardgameStore = boardgameStore;
            _matchmakingRuleStore = matchmakingRuleStore;
            _boardgameRuleStore = boardgameRuleStore;
        }

        [BindProperty(SupportsGet = true)]
        public string RuleId { get; set; }

        public BoardgameRule BoardgameRule { get; set; }

        public MatchmakingRule MatchmakingRule { get; set; }

        public BoardgameModel Boardgame { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Boardgame = await _boardgameRuleStore.ReturnBoardgameByMatchmakingIdAsync(RuleId);
            BoardgameRule = await _boardgameRuleStore.FindBoardgameRuleByIdAsync(Boardgame.Id, RuleId);
            MatchmakingRule = await _matchmakingRuleStore.FindRuleByIdAsync(RuleId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BoardgameModel boardgame = await _boardgameRuleStore.ReturnBoardgameByMatchmakingIdAsync(RuleId);
                    MatchmakingRule rule = await _matchmakingRuleStore.FindRuleByIdAsync(RuleId);
                    BoardgameRule boardgameRule = await _boardgameRuleStore.FindBoardgameRuleByIdAsync(boardgame.Id, rule.Id);

                    IdentityResult deletionResult = await _boardgameRuleStore.DeleteBoardgameRuleAsync(boardgame, rule);
                    if (deletionResult.Succeeded)
                    {
                        MatchmakingRule matchmakingRule = await _matchmakingRuleStore.FindRuleByIdAsync(RuleId);

                        IdentityResult identityResult = await _matchmakingRuleStore.DeleteRuleAsync(matchmakingRule);
                        if (identityResult.Succeeded)
                        {
                            return RedirectToPage("ModifyBoardgame", new { BoardgameID = boardgame.BGGId });
                        }
                        else
                        {
                            StatusMessage = $"Error during deleting a matchmaking rule - try again later";
                            return Page();
                        }
                    }
                    else
                    {
                        StatusMessage = $"Error during deleting a matchmaking rule - try again later";
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
    }
}