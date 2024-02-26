#nullable disable

using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Tls;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    public class EditBindingModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }     

        public bool PhoneNumberConfirmed { get; set; }

        public string BGGUsername { get; set; }

        public string UserDescription { get; set; }
    }

    public class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ApplicationUser ApplicationUser;

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("./ListUsers");
            }

            ApplicationUser = await _userManager.FindByIdAsync(Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            [FromForm(Name = "ApplicationUser")] EditBindingModel editBinding)
        {
            if (!string.IsNullOrEmpty(Id) && ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(Id);

                if(User != null)
                {
                    user.UserName = editBinding.UserName;
                    user.FirstName = editBinding.FirstName;
                    user.LastName = editBinding.LastName;
                    user.Email = editBinding.Email;
                    user.EmailConfirmed = editBinding.EmailConfirmed;
                    user.PhoneNumber = editBinding.PhoneNumber;
                    user.PhoneNumberConfirmed = editBinding.PhoneNumberConfirmed;
                    user.BGGUsername = editBinding.BGGUsername;
                    user.UserDescription = editBinding.UserDescription;
                }

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("ManageUsers");
                }
                else 
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                ApplicationUser = await _userManager.FindByIdAsync(Id);
                return Page();
            }
            return Page();
        }
    }
}
