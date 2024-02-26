#nullable disable
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Boardgame
{
    public class RemoveFromFavouriteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;

        public RemoveFromFavouriteModel(UserManager<ApplicationUser> userManager, IBoardGameStore<BoardgameModel> boardgameStore)
        {
            _userManager = userManager;
            _boardgameStore = boardgameStore;
        }

        [BindProperty(SupportsGet = true)]
        public int BoardgameID { get; set; }

        [BindProperty]
        public List<string> BoardgameFavourites { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public BoardgameModel Boardgame { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(BoardgameID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser = await _userManager.GetUserAsync(User);
                Boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(BoardgameID);

                // remove entity to end of the list 
                BoardgameFavourites.Remove(Boardgame.BGGId.ToString());
                IdentityResult result = await _userManager.SetUserFavouriteBoardGamesAsync(ApplicationUser, BoardgameFavourites);
                if (result.Succeeded)
                {
                    return RedirectToPage("Index", new { BoardgameID = BoardgameID });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Page();
        }
    }
}
