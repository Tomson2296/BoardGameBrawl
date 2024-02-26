#nullable disable
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Mailbox
{
    public class SendMessageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;

        public SendMessageModel(UserManager<ApplicationUser> userManager,
            IUserFriendStore<UserFriend, ApplicationUser> userFriendService)
        {
            _userManager = userManager;
            _userFriendStore = userFriendService;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public IEnumerable<BasicUserInfoDTO> FriendsList { get; set; } = new List<BasicUserInfoDTO>();
        
        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            FriendsList = await _userFriendStore.GetUserFriendsAsync(ApplicationUser.Id);
            return Page();
        }
    }
}
