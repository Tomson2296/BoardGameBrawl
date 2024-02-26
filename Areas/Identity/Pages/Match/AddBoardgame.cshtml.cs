#nullable disable
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BoardGameBrawl.Areas.Identity.Pages.Match
{
    public class AddBoardgameModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;

        public AddBoardgameModel(UserManager<ApplicationUser> userManager, IBoardGameStore<BoardgameModel> boardgameStore)
        {
            _userManager = userManager;
            _boardgameStore = boardgameStore;
        }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty]
        public string Chosen_BoardgameId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<BoardgameDTO> Filtered_Boardgames { get; set; } = new List<BoardgameDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!Filter.IsNullOrEmpty())
            {
                Filtered_Boardgames = await _boardgameStore.GetBoardgamesDTOByFilterAsync(Filter);
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("AddBoardgame", new { Filter });
        }

        public async Task<IActionResult> OnPostSetBoardgameAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                string chosenBoardgame = Request.Form["BoardgameId"];
                HttpContext.Session.SetString("Chosen_Boardgame", chosenBoardgame);
                return RedirectToPage("CreateMatch");
            }
            else
            {
                return Page();    
            }
        }
    }
}
