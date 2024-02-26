#nullable disable
using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User
{
    public class BGGCollectionModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBGGAPIService _BGGAPIService;

        public BGGCollectionModel(UserManager<ApplicationUser> userManager, IBGGAPIService BGGAPIService)
        {
            _userManager = userManager;
            _BGGAPIService = BGGAPIService;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public BoardGameCollection UserBoardGameCollection { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (ApplicationUser.BGGUsername != null)
            {
                UserBoardGameCollection = await _BGGAPIService.GetUserBGGCollectionInfo(ApplicationUser.BGGUsername);
            }
            return Page();
        }
    }
}
