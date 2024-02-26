#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        private readonly IUserScheduleStore<UserSchedule, ApplicationUser> _userScheduleStore;
        private readonly IUserNotificationStore<UserNotification, ApplicationUser> _userNotificationStore;
        private readonly IUserRatingStore<UserRating, ApplicationUser, BoardgameModel> _userRatingStore;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;
        private readonly IUserGeolocationStore<UserGeolocation, ApplicationUser> _userGeolocationStore;
        private readonly IMessageStore<MessageModel, ApplicationUser> _messageStore;
        private readonly IMatchStore<MatchModel, BoardgameModel, ApplicationUser> _matchStore;
        private readonly IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> _groupParticipantStore;


        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,

            IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore,
            IUserNotificationStore<UserNotification, ApplicationUser> userNotificationStore,
            IUserRatingStore<UserRating, ApplicationUser, BoardgameModel> userRatingStore,
            IUserFriendStore<UserFriend, ApplicationUser> userFriendStore,
            IUserGeolocationStore<UserGeolocation, ApplicationUser> userGeolocationStore,
            IMessageStore<MessageModel, ApplicationUser> messageStore,
            IMatchStore<MatchModel, BoardgameModel, ApplicationUser> matchStore,
            IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> groupParticipantStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userScheduleStore = userScheduleStore;
            _userNotificationStore = userNotificationStore;
            _userRatingStore = userRatingStore;
            _userFriendStore = userFriendStore;
            _userGeolocationStore = userGeolocationStore;
            _messageStore = messageStore;
            _matchStore = matchStore;
            _groupParticipantStore = groupParticipantStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return Redirect("~/");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return Redirect("~/");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
