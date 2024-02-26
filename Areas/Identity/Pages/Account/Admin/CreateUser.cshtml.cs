#nullable disable

using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<CreateUserModel> _logger;
        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;

        public CreateUserModel(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<CreateUserModel> logger,
            IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore
            )
        {
            _userManager = userManager;
            _logger = logger;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _userScheduleStore = userScheduleStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Phone]
            [DataType(DataType.PhoneNumber)]
            public string Phone { get; set; }   

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string BGGUsername { get; set; }
            
            public string UserDescription { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                var creationDate = DateOnly.FromDateTime(DateTime.Now);

                // Set obligatory data about new user - Username, Email, Password
                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                await _userManager.AddPasswordAsync(user, Input.Password);

                // Set creation Time 
                await _userStore.SetUserDateTimeCreationAsync(user, creationDate, CancellationToken.None);

                // Check and set additional info about user

                if (Input.Phone != null)
                {
                    await _userManager.SetPhoneNumberAsync(user, Input.Phone);
                }

                if (Input.FirstName != null)
                {
                    await _userStore.SetUserFirstNameAsync(user, Input.FirstName, CancellationToken.None);
                }

                if (Input.LastName != null)
                {
                    await _userStore.SetUserLastNameAsync(user, Input.LastName, CancellationToken.None);
                }

                if (Input.BGGUsername != null)
                {
                    await _userStore.SetBGGUsernameAsync(user, Input.BGGUsername, CancellationToken.None);
                }

                if (Input.UserDescription != null)
                {
                    await _userStore.SetUserDescriptionAsync(user, Input.UserDescription, CancellationToken.None);
                }

                // set email
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, Input.Password);

                // create deafult instance of userSchedule
                UserSchedule userSchedule = CreateUserSchedule();
                await _userScheduleStore.SetUserByAsync(userSchedule, user);
                await _userScheduleStore.CreateScheduleAsync(userSchedule);
                _logger.LogInformation("Default UserSchedule has been added to account.");

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new user.");

                    // Adding newly created user to User role in application 
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("Default credentials to account (Role : User) has been added to account.");

                    StatusMessage = "User has been created successfully";
                    return Page();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    StatusMessage = "Error during cration process. Try again.";
                    return Page();
                }
            }

            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'.");
            }
        }
        private UserSchedule CreateUserSchedule()
        {
            try
            {
                return Activator.CreateInstance<UserSchedule>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserSchedule)}'.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
