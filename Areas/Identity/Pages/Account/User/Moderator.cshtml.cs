#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User
{
    public class ModeratorPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;

        public ModeratorPageModel(UserManager<ApplicationUser> userManager, IBoardGameStore<BoardgameModel> boardgameStore)
        {
            _userManager = userManager;
            _boardgameStore = boardgameStore;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public IList<BoardgameModel> Boardgames { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Boardgames = await GetListofBoardgames(ApplicationUser);
            return Page();
        }

        private async Task<IList<BoardgameModel>> GetListofBoardgames(ApplicationUser applicationUser)
        {
            IList<Claim> Claims = await _userManager.GetClaimsAsync(applicationUser);
            IList<Claim> ModerationClaims = Claims.Where(c => c.Type == "BoardGameModerationPermission").ToList();

            List<BoardgameModel> result = new List<BoardgameModel>();
            foreach (var item in ModerationClaims)
            {
                BoardgameModel boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(int.Parse(item.Value));
                result.Add(boardgame);
            }
            return result;
        }
    }
}