#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.AppUser
{
    public class UserBGGCollectionModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBGGAPIService _BGGAPIService;

        public UserBGGCollectionModel(UserManager<ApplicationUser> userManager, IBGGAPIService BGGAPIService)
        {
            _userManager = userManager;
            _BGGAPIService = BGGAPIService;
        }

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public BoardGameCollection UserBoardGameCollection { get; set; } = new BoardGameCollection();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.FindByNameAsync(UserName);
            if (ApplicationUser == null)
            {
                return NotFound($"User with that Username: {UserName} has not been found.");
            }

            if (ApplicationUser.BGGUsername != null)
            {
                UserBoardGameCollection = await _BGGAPIService.GetUserBGGCollectionInfo(ApplicationUser.BGGUsername);
            }
            return Page();
        }
    }
}
