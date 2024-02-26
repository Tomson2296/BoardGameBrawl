#nullable disable

using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class ViewUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ViewUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }  

        // get property names for ApplicationUser and save in IEnumerable object
        public IEnumerable<string> PropertyNames
            => typeof(ApplicationUser).GetProperties()
                .Select(prop => prop.Name);

        // get value of the property 
        public string GetValue(string name)
            => typeof(ApplicationUser).GetProperty(name).GetValue(ApplicationUser)?.ToString();

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("ManageUsers");
            }

            ApplicationUser = await _userManager.FindByIdAsync(Id);
            if(ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}
