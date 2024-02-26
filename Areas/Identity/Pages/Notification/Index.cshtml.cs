#nullable disable
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BoardGameBrawl.Areas.Identity.Pages.Notification
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserNotificationStore<UserNotification, ApplicationUser> _userNotificationStore;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            IUserNotificationStore<UserNotification, ApplicationUser> userNotificationStore,
            IUserFriendStore<UserFriend, ApplicationUser> userFriendStore,
            IBoardGameStore<BoardgameModel> boardgameStore)
        {
            _userManager = userManager;
            _userNotificationStore = userNotificationStore;
            _userFriendStore = userFriendStore;
            _boardgameStore = boardgameStore;
        }

        [BindProperty]
        public IEnumerable<UserNotification> UserNotification { get; set; } = new List<UserNotification>();

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserNotification = await _userNotificationStore.FindNotificationsByReceiverIdAsync(ApplicationUser.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAcceptFriendsInvitation()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    string notificationId = Request.Form["notificationID"];

                    // get notification by id
                    UserNotification notification = await _userNotificationStore.FindNotificationByIdAsync(notificationId);
                    List<string> notificationSender = notification.Notification.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

                    // find sender of particular notification in database
                    ApplicationUser sender = await _userManager.FindByNameAsync(notificationSender[0]);

                    // get friendship and change its status to active - save to database context
                    UserFriend friendship = await _userFriendStore.FindFriendshipByUserFriendIdsAsync(sender.Id, user.Id);
                    await _userFriendStore.SetFriendshipAcceptedAsync(friendship, true);
                    await _userFriendStore.UpdateFriendshipAsync(friendship);

                    // set notification to shown - save to database context
                    await _userNotificationStore.SetNotificationShown(notification, true);
                    await _userNotificationStore.UpdateNotificationAsync(notification);

                    // create a new notification - sent info back about accepting invetation
                    UserNotification responseNotification = CreateUserNotification();
                    string notificationText = $"{user.UserName} accepted your invitation to Friend List";
                    NotificationType notificationType = NotificationType.Response;

                    await _userNotificationStore.SetReceiverAsync(responseNotification, sender);
                    await _userNotificationStore.SetNotificationAsync(responseNotification, notificationText);
                    await _userNotificationStore.SetNotificationShown(responseNotification, true);
                    await _userNotificationStore.SetNotificationTypeAsync(responseNotification, notificationType);

                    // Create a new notification object and save it in application context
                    await _userNotificationStore.CreateNotificationAsync(responseNotification);

                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error during accepting friendship - {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAcceptUserToModeratorGroup()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    string notificationId = Request.Form["notificationID"];

                    // get notification by id
                    UserNotification notification = await _userNotificationStore.FindNotificationByIdAsync(notificationId);
                    List<string> notificationSender = notification.Notification.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

                    // find sender of particular notification in database
                    ApplicationUser sender = await _userManager.FindByNameAsync(notificationSender[0]);

                    // give user a moderator role
                    await _userManager.AddToRoleAsync(sender, "Moderator");
                    await _userManager.UpdateAsync(sender);

                    // set notification to shown - save to database context
                    await _userNotificationStore.SetNotificationShown(notification, true);
                    await _userNotificationStore.UpdateNotificationAsync(notification);

                    // create a new notification - sent info back about accepting invetation
                    UserNotification responseNotification = CreateUserNotification();
                    string notificationText = $"You have received Moderator privileges in application.";
                    NotificationType notificationType = NotificationType.Response;

                    await _userNotificationStore.SetReceiverAsync(responseNotification, sender);
                    await _userNotificationStore.SetNotificationAsync(responseNotification, notificationText);
                    await _userNotificationStore.SetNotificationShown(responseNotification, true);
                    await _userNotificationStore.SetNotificationTypeAsync(responseNotification, notificationType);

                    // Create a new notification object and save it in application context
                    await _userNotificationStore.CreateNotificationAsync(responseNotification);

                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error during accepting user to moderator role - {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }

       public async Task<IActionResult> OnPostAcceptUserToBoardgameModeration()
       {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    string notificationId = Request.Form["notificationID"];

                    // get notification by id
                    UserNotification notification = await _userNotificationStore.FindNotificationByIdAsync(notificationId);
                    List<string> notificationSender = notification.Notification.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

                    // find sender of particular notification in database
                    ApplicationUser sender = await _userManager.FindByNameAsync(notificationSender[0]);
                    BoardgameModel boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(int.Parse(notificationSender[6]));

                    // create a new claim to user -> permission to moderate a specified boardgame
                    BoardgameClaim boardgameClaim = new BoardgameClaim() { BoardGameId = boardgame.BGGId };
                    var claim = new Claim(boardgameClaim.ClaimType, boardgameClaim.BoardGameId.ToString(), ClaimValueTypes.String);
                    await _userManager.AddClaimAsync(sender, claim);

                    // set notification to shown - save to database context
                    await _userNotificationStore.SetNotificationShown(notification, true);
                    await _userNotificationStore.UpdateNotificationAsync(notification);

                    // create a new notification - sent info back about accepting invetation
                    UserNotification responseNotification = CreateUserNotification();
                    string notificationText = $"You have been accepted to {boardgame.Name} Moderator Group.";
                    NotificationType notificationType = NotificationType.Response;

                    await _userNotificationStore.SetReceiverAsync(responseNotification, sender);
                    await _userNotificationStore.SetNotificationAsync(responseNotification, notificationText);
                    await _userNotificationStore.SetNotificationShown(responseNotification, true);
                    await _userNotificationStore.SetNotificationTypeAsync(responseNotification, notificationType);

                    // Create a new notification object and save it in application context
                    await _userNotificationStore.CreateNotificationAsync(responseNotification);

                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error during adding user to boardgame moderation group - {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }

        //public async Task<IActionResult> OnPostAcceptMatchInvitation(string notificationId)
        //{

        //}

        public async Task<IActionResult> OnPostDeclineNotification()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    string notificationId = Request.Form["notificationID"];

                    // get notification by id
                    UserNotification notification = await _userNotificationStore.FindNotificationByIdAsync(notificationId);

                    // set notification to shown - save to database context
                    await _userNotificationStore.SetNotificationShown(notification, true);
                    await _userNotificationStore.UpdateNotificationAsync(notification);

                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error during declining notification - {ex.Message}";
                    return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteNotification()
        {
            if (ModelState.IsValid)
            {
                string notificationId = Request.Form["notificationID"];
                UserNotification notification = await _userNotificationStore.FindNotificationByIdAsync(notificationId);
                IdentityResult result = await _userNotificationStore.DeleteNotificationAsync(notification);
                if (result.Succeeded)
                {
                    return Page();
                }
                else
                {
                    StatusMessage = "Error during process of removing a notification. Try again later.";
                    return Page();
                }
            }
            return Page();
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