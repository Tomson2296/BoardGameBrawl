#nullable disable
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace BoardGameBrawl.Areas.Identity.Pages.Boardgame
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBGGAPIService _BGGAPIService;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;

        public IndexModel(UserManager<ApplicationUser> userManager,
            IBGGAPIService BGGAPIService,
            IBoardGameStore<BoardgameModel> boardgameStore)
        {
            _userManager = userManager;
            _BGGAPIService = BGGAPIService;
            _boardgameStore = boardgameStore;
        }

        [BindProperty(SupportsGet = true)]
        public int BoardgameID { get; set; }

        [BindProperty]
        public BoardgameItem Boardgame { get; set; }

        public bool IsBoardgameExistsInDB { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public bool IsBoardgameUsersfavourite { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IList<string> BoardgameDescription { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Boardgame = await _BGGAPIService.GetBGGBoardGameInfo(BoardgameID);
            BoardgameDescription = await GetBoardgameDescription(Boardgame.GameInfo.Description);

            IsBoardgameExistsInDB = await _boardgameStore.CheckIsBoardgameExistsInDBAsync(BoardgameID);
            if (!IsBoardgameExistsInDB)
            {
                await AddBoardgameToDatabase(Boardgame);
            }
            IsBoardgameUsersfavourite = await CheckIfBGFavourite(Boardgame.GameInfo.Id.ToString());
            return Page();
        }

        private async Task<bool> CheckIfBGFavourite(string boardgameId)
        {
            List<string> BoardgameFavourites = await _userManager.GetUserFavouriteBoardGamesAsync(ApplicationUser);

            if (BoardgameFavourites.IsNullOrEmpty())
                return false;

            foreach (string boardgame in BoardgameFavourites)
            {
                if (boardgame == boardgameId)
                    return true;
            }
            return false;
        }

        private async Task<IList<string>> GetBoardgameDescription(string boardgameDesc)
        {
            IList<string> modifiedDescString = new List<string>();
            
            string breakingLineString = "&#10;";

            if (boardgameDesc.Contains(breakingLineString))
            {
                modifiedDescString = boardgameDesc.Split(breakingLineString);
            }

            for (int i = 0; i < modifiedDescString.Count; i++)
            {
                string oldString = modifiedDescString.ElementAt(i).ToString();
                if (oldString.IsNullOrEmpty()) continue;
                string decodedString = HttpUtility.HtmlDecode(modifiedDescString.ElementAt(i));
                modifiedDescString[i] = decodedString;
            }
            return await Task.FromResult(modifiedDescString);
        }

        private async Task<IActionResult> AddBoardgameToDatabase(BoardgameItem boardgameItem)
        {
            if (ModelState.IsValid)
            {
                BoardgameModel boardgameModel = CreateNewBoardgameModel();

                await _boardgameStore.SetBoardGameNameAsync(boardgameModel, boardgameItem.GameInfo.Name.Value);
                await _boardgameStore.SetBoardGameBGGIdAsync(boardgameModel, boardgameItem.GameInfo.Id);
                await _boardgameStore.SetBoardGameMinPlayersAsync(boardgameModel, boardgameItem.GameInfo.MinPlayers.Value);
                await _boardgameStore.SetBoardGameMaxPlayersAsync(boardgameModel, boardgameItem.GameInfo.MaxPlayers.Value);
                await _boardgameStore.SetBoardGameYearPublishedAsync(boardgameModel, boardgameItem.GameInfo.YearPublished.Value);
                await _boardgameStore.SetBoardGameImageFileNameAsync(boardgameModel, boardgameItem.GameInfo.Thumbnail);

                List<string> categories = boardgameItem.GameInfo.Link.Where(l => l.Type.Equals("boardgamecategory")).Select(l => l.Value).ToList();
                List<string> mechanics = boardgameItem.GameInfo.Link.Where(l => l.Type == "boardgamemechanic").Select(l => l.Value).ToList();

                await _boardgameStore.SetBoardgameCategoriesAsync(boardgameModel, categories);
                await _boardgameStore.SetBoardgameMechanicsAsync(boardgameModel, mechanics);

                IdentityResult result = await _boardgameStore.CreateAsync(boardgameModel, CancellationToken.None);

                if (result.Succeeded)
                {
                    return Page();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        StatusMessage = "Error during process of adding a boardgame to database. Try again later.";
                        ModelState.AddModelError(string.Empty, error.Description);
                        return Page();
                    }
                }
            }
            return Page();
        }

        private BoardgameModel CreateNewBoardgameModel()
        {
            try
            {
                return Activator.CreateInstance<BoardgameModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BoardgameModel)}'.");
            }
        }
    }
}
