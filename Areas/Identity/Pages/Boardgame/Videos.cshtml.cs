#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BoardGameBrawl.Areas.Identity.Pages.Boardgame
{
    public class VideosModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBGGAPIService _BGGAPIService;

        public VideosModel(UserManager<ApplicationUser> userManager, IBGGAPIService bggAPIService)
        {
            _userManager = userManager;
            _BGGAPIService = bggAPIService;
        }

        [BindProperty(SupportsGet = true)]
        public int BoardgameID { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Language { get; set; }

        [BindProperty]
        public BoardgameItem Boardgame { get; set; }

        [BindProperty]
        public IEnumerable<string> LanguageSelection { get; set; } = new List<string>();

        [BindProperty]
        public IEnumerable<Video> InstructionalVideos { get; set; } = new List<Video>(); 

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Boardgame = await _BGGAPIService.GetBGGBoardGameInfo(BoardgameID);

            LanguageSelection = GetLanguages(Boardgame);

            if (Language.IsNullOrEmpty())
            {
                return Page();
            }
            else 
            {
                InstructionalVideos = GetInstructionalVideos(Boardgame, Language);
                return Page();
            }
        }

        public IActionResult OnPostAsync()
        {
            return RedirectToPage("Videos", new { Language });
        }


        private IEnumerable<string> GetLanguages(BoardgameItem boardgameItem)
        {
            return boardgameItem.GameInfo.Videos.VideoList.Select(v => v.Language).Distinct().ToList();
        }

        private IEnumerable<Video> GetInstructionalVideos(BoardgameItem boardgameItem, string language)
        {
            return boardgameItem.GameInfo.Videos.VideoList.Where(v => v.Language == language).ToList();
        }
    }
}
