#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "BGGUsername")]
            public string BGGUsername { get; set; }

            [Display(Name = "UserDescription")]
            public string UserDescription { get; set; }

            [Display(Name = "Profile Avatar")]
            public byte[] ProfileAvatar { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = await _userManager.GetUserFirstNameAsync(user);
            var lastName = await _userManager.GetUserLastNameAsync(user);
            var bggUsername = await _userManager.GetBGGUserNameAsync(user);
            var userDescription = await _userManager.GetUserDescriptionAsync(user);
            var profileAvatar = await _userManager.GetUserAvatarAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                BGGUsername = bggUsername,
                UserDescription = userDescription,
                ProfileAvatar = profileAvatar
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var firstName = await _userManager.GetUserFirstNameAsync(user);
            if (Input.FirstName != firstName)
            {
                var setFirstname = await _userManager.SetUserFirstNameAsync(user, Input.FirstName);
                if (!setFirstname.Succeeded)
                {
                    StatusMessage = "Error when trying to set first name.";
                    return RedirectToPage();
                }
            }

            var lastName = await _userManager.GetUserLastNameAsync(user);
            if (Input.LastName != lastName)
            {
                var setLastName = await _userManager.SetUserLastNameAsync(user, Input.LastName);
                if (!setLastName.Succeeded)
                {
                    StatusMessage = "Error when trying to set last name.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var bggUsername = await _userManager.GetBGGUserNameAsync(user);
            if (Input.BGGUsername != bggUsername)
            {
                var setBGGUsername = await _userManager.SetBGGUserNameAsync(user, Input.BGGUsername);
                if (!setBGGUsername.Succeeded)
                {
                    StatusMessage = "Error when trying to set BGGUsername.";
                    return RedirectToPage();
                }
            }

            var userDescription = await _userManager.GetUserDescriptionAsync(user);
            if (Input.UserDescription != userDescription)
            {
                var setUserDescription = await _userManager.SetUserDescriptionAsync(user, Input.UserDescription);
                if (!setUserDescription.Succeeded)
                {
                    StatusMessage = "Error when trying to set user description.";
                    return RedirectToPage();
                }
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    // Upload the file if less than 2 MB
                    if (dataStream.Length < 2097152)
                    {
                        await file.CopyToAsync(dataStream);
                        user.UserAvatar = dataStream.ToArray();
                        await _userManager.SetUserAvatarAsync(user, user.UserAvatar);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }

            await _context.SaveChangesAsync();
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
