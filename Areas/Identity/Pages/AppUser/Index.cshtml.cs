#nullable disable

using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.AppUser
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.FindByNameAsync(UserName);
            if (ApplicationUser == null)
            {
                return NotFound($"User with that Username: {UserName} has not been found.");
            }
            return Page();
        }
    }
}