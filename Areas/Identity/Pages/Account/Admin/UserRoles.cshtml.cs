#nullable disable

using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class UserRolesModel : PageModel
    {
        public class ViewUserModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
        }

        public UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;

        public UserRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDeleteFromListAsync(string role)
        {
            IdentityRole idRole = await _roleManager.FindByNameAsync(role);
            IdentityResult result = await _roleManager.DeleteAsync(idRole);

            if (result.Succeeded)
            {
                StatusMessage = "Role has been deleted successfully.";
                return RedirectToPage();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    StatusMessage = "Error - Role has not been deleted. Try again.";
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToListAsync(string role)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded)
            {
                StatusMessage = "Role successfully added.";
                return RedirectToPage();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    StatusMessage = "Error - Role have not been added. Try again.";
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }
            return Page();
        }
    }
}
