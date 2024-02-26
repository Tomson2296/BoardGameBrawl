#nullable disable
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User
{
    public class BoardgameFavouritesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;

        public BoardgameFavouritesModel(UserManager<ApplicationUser> userManager,
            IBoardGameStore<BoardgameModel> boardGameStore)
        {
            _userManager = userManager;
            _boardgameStore = boardGameStore;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public List<string> BoardgameFavouritesList { get; set; } = new List<string>();

        [BindProperty]
        public IEnumerable<BoardgameModel> BoardgameFavourites { get; set; } = new List<BoardgameModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            BoardgameFavouritesList = await _userManager.GetUserFavouriteBoardGamesAsync(ApplicationUser);
            
            if (BoardgameFavouritesList == null)
            {
                return Page();
            }
            else
            {
                BoardgameFavourites = await GetUserFavouriteBoardgames();
                return Page();
            }
        }

        private async Task<IEnumerable<BoardgameModel>> GetUserFavouriteBoardgames()
        {
            List<BoardgameModel> result = new List<BoardgameModel>();
            foreach (var boardgameID in BoardgameFavouritesList)
            {
                result.Add(await _boardgameStore.FindBoardGameByBGGIdAsync(int.Parse(boardgameID)));
            }
            return result;
        }
    }
}
