#nullable disable

using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class ChangeUserPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailKitService _mailKitService;

        public ChangeUserPasswordModel(UserManager<ApplicationUser> userManager, IMailKitService mailKitService)
        {
            _userManager = userManager;
            _mailKitService = mailKitService;
        }

        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmationPassword { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("./Passwords", new { PageNumber });
            }

            ApplicationUser User = await _userManager.FindByIdAsync(Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = await _userManager.FindByIdAsync(Id);
                if (await _userManager.HasPasswordAsync(User))
                {
                    await _userManager.RemovePasswordAsync(User);
                }

                IdentityResult result =
                    await _userManager.AddPasswordAsync(User, NewPassword);

                if (result.Succeeded)
                {
                    await _mailKitService.SendEmailAsync(User.Email, "Message from site Administration. Your password has been changed." +
                        "Please, sign-up to aplication using password sent in that message. You can later change it in your profile panel. " +
                        "Thank you.", NewPassword);
                    return RedirectToPage("./Passwords", new { PageNumber });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return Page();
                    }
                }
            }
            return Page();
        }
    }
}
