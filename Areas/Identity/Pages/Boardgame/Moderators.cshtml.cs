#nullable disable
using AutoMapper;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BoardGameBrawl.Areas.Identity.Pages.Boardgame
{
    public class ModeratorsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBoardGameStore<BoardgameModel> _boardgameStore;
        private readonly IUserNotificationStore<UserNotification, ApplicationUser> _userNotificationStore;
        private readonly IMapper _mapper;

        public ModeratorsModel(UserManager<ApplicationUser> userManager,
            IBoardGameStore<BoardgameModel> boardGameStore,
            IUserNotificationStore<UserNotification, ApplicationUser> userNotificationStore,
            IMapper mapper)
        {
            _userManager = userManager;
            _boardgameStore = boardGameStore;
            _userNotificationStore = userNotificationStore;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int BoardgameID { get; set; }

        [BindProperty]
        public ApplicationUser ActiveUser { get; set; }

        [BindProperty]
        public bool IsModeratingPrivileges { get; set; }

        [BindProperty]
        public IList<Claim> User_Claims { get; set; }

        public BoardgameModel Boardgame { get; set; }

        [BindProperty]
        public IList<BasicUserInfoDTO> ModeratorsList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveUser = await _userManager.GetUserAsync(User);
            if (ActiveUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            User_Claims = await _userManager.GetClaimsAsync(ActiveUser);
            IsModeratingPrivileges = CheckIfModeratingPrivileges(User_Claims, BoardgameID);
            ModeratorsList = await GetBoardgameModeratorsList(BoardgameID);
            Boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(BoardgameID);
            return Page();
        }

        private static bool CheckIfModeratingPrivileges(IList<Claim> claims, int boardgameId)
        {
            var result = claims.SingleOrDefault(c => c.Type == "BoardGameModerationPermission" && c.Value == boardgameId.ToString());
            if (result != default)
                return true;
            else
                return false;
        }

        private async Task<IList<BasicUserInfoDTO>> GetBoardgameModeratorsList(int boardgameId)
        {
            List<BasicUserInfoDTO> result = new List<BasicUserInfoDTO>();

            // search for legitimate moderators in database
            IEnumerable<ApplicationUser> moderators = await _userManager.GetUsersInRoleAsync("Moderator");

            // iterate through available moderators to find those with appropriate claim
            foreach (ApplicationUser moderator in moderators)
            {
                IList<Claim> moderatorClaims = await _userManager.GetClaimsAsync(moderator);
                var moderationEligible = moderatorClaims.SingleOrDefault(c => c.Type == "BoardGameModerationPermission" && c.Value == boardgameId.ToString());

                if (moderationEligible != default)
                    result.Add(_mapper.Map<BasicUserInfoDTO>(moderator));
                else
                    continue;
            }
            return result;
        }

        public async Task<IActionResult> OnPostAddUserToModeratorsGroup()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // sending notification to administrator - administrator is receiver
                    ApplicationUser admin = await _userManager.FindByNameAsync("admin");
                    ApplicationUser activeUser = await _userManager.GetUserAsync(User);

                    UserNotification userNotification = CreateUserNotification();
                    string notificationText = $"{activeUser.UserName} wants to get moderator priviliges - accept?";
                    NotificationType notificationType = NotificationType.AddToModeratorRole;

                    await _userNotificationStore.SetReceiverAsync(userNotification, admin);
                    await _userNotificationStore.SetNotificationAsync(userNotification, notificationText);
                    await _userNotificationStore.SetNotificationShown(userNotification, false);
                    await _userNotificationStore.SetNotificationTypeAsync(userNotification, notificationType);

                    // Create a new notification object and save it in application context
                    IdentityResult userNotificationCreationResult = await _userNotificationStore.CreateNotificationAsync(userNotification);

                    if (userNotificationCreationResult.Succeeded)
                    {
                        return RedirectToPage("Index", new { BoardgameID = BoardgameID });
                    }
                    else
                    {
                        foreach (var error in userNotificationCreationResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                            return Page();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddUserToBoardGameModeration()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // get value from form and get boardgame name from database 
                    string receiverUserName = Request.Form["receiver"];

                    // sending notification to particular user 
                    ApplicationUser receiver = await _userManager.FindByNameAsync(receiverUserName);
                    ApplicationUser activeUser = await _userManager.GetUserAsync(User);
                    BoardgameModel boardgame = await _boardgameStore.FindBoardGameByBGGIdAsync(BoardgameID);

                    // create a new user notification object 
                    UserNotification userNotification = CreateUserNotification();
                    string notificationText = $"{activeUser.UserName} wants to join moderator group {boardgame.BGGId} - accept?";
                    NotificationType notificationType = NotificationType.AddToBoardGameModeration;

                    await _userNotificationStore.SetReceiverAsync(userNotification, receiver);
                    await _userNotificationStore.SetNotificationAsync(userNotification, notificationText);
                    await _userNotificationStore.SetNotificationShown(userNotification, false);
                    await _userNotificationStore.SetNotificationTypeAsync(userNotification, notificationType);

                    // Create a new notification object and save it in application context
                    IdentityResult userNotificationCreationResult = await _userNotificationStore.CreateNotificationAsync(userNotification);

                    if (userNotificationCreationResult.Succeeded)
                    {
                        return RedirectToPage("Index", new { BoardgameID = BoardgameID });
                    }
                    else
                    {
                        foreach (var error in userNotificationCreationResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                            return Page();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
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