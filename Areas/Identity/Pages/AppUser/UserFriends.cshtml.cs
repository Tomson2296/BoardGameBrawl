#nullable disable
using BoardGameBrawl.Data.Models.API_XML;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.AppUser
{
    public class UserFriendsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;
        private readonly IUserNotificationStore<UserNotification, ApplicationUser> _userNotificationStore;

        public UserFriendsModel(UserManager<ApplicationUser> userManager,
            IUserFriendStore<UserFriend, ApplicationUser> userFriendStore,
            IUserNotificationStore<UserNotification, ApplicationUser> userNotificationStore)
        {
            _userManager = userManager;
            _userFriendStore = userFriendStore;
            _userNotificationStore = userNotificationStore;
        }
        
        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }

        public ApplicationUser ActiveUser { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IEnumerable<BasicUserInfoDTO> UserFindFriends { get; set; } = new List<BasicUserInfoDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveUser = await _userManager.GetUserAsync(User);
            if (ActiveUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ApplicationUser = await _userManager.FindByNameAsync(UserName);
            if (ApplicationUser == null)
            {
                return NotFound($"User with that Username: {UserName} has not been found.");
            }
            UserFindFriends = await _userFriendStore.GetUserFriendsAsync(ApplicationUser.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAddUserToFriends()
        {
            if (ModelState.IsValid)
            {
                ActiveUser = await _userManager.GetUserAsync(User);
                ApplicationUser = await _userManager.FindByNameAsync(UserName);

                //Create a new UserFriend object that represent a new frienship relation between users 
                //Set IsAccepted boolean value to false (as default behaviour)
                UserFriend userFriend = CreateUserFriendObject();

                await _userFriendStore.SetUserAsync(userFriend, ActiveUser);
                await _userFriendStore.SetFriendAsync(userFriend, ApplicationUser);
                await _userFriendStore.SetFriendshipAcceptedAsync(userFriend, false);

                // Create a new friendship object and save it in application context
                IdentityResult userFriendCreationResult = await _userFriendStore.CreateFriendshipAsync(userFriend);

                if (!userFriendCreationResult.Succeeded)
                {
                    foreach (var error in userFriendCreationResult.Errors)
                    {
                        StatusMessage = "Error during process of adding a uset to friendsList. Try again later.";
                        ModelState.AddModelError(string.Empty, error.Description);
                        return Page();
                    }
                }
                else
                {
                    // Create a new notification for a user who has been invited to friend list
                    // Set IsShown boolean value to false (as default behaviour)
                    UserNotification userNotification = CreateUserNotification();
                    string notificationText = $"{ActiveUser.UserName} wants to add you to Friend List. Do you accept invitation?";
                    NotificationType notificationType = NotificationType.AddToFriends;

                    await _userNotificationStore.SetReceiverAsync(userNotification, ApplicationUser);
                    await _userNotificationStore.SetNotificationAsync(userNotification, notificationText);
                    await _userNotificationStore.SetNotificationShown(userNotification, false);
                    await _userNotificationStore.SetNotificationTypeAsync(userNotification, notificationType);

                    // Create a new notification object and save it in application context
                    IdentityResult userNotificationCreationResult = await _userNotificationStore.CreateNotificationAsync(userNotification);

                    if (userNotificationCreationResult.Succeeded)
                    {
                        StatusMessage = "Invitation to friend list has been sent.";
                        return RedirectToPage();
                    }
                    else
                    {
                        foreach (var error in userFriendCreationResult.Errors)
                        {
                            StatusMessage = "Error during process of adding a user to friendsList. Try again later.";
                            ModelState.AddModelError(string.Empty, error.Description);
                            return Page();
                        }
                    }
                }
                return Page();
            }
            return Page();
        }

        private UserFriend CreateUserFriendObject()
        {
            try
            {
                return Activator.CreateInstance<UserFriend>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserFriend)}'.");
            }
        }

        private UserNotification CreateUserNotification()
        {
            try
            {
                return Activator.CreateInstance<UserNotification>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserNotification)}'.");
            }
        }
    }
}