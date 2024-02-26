#nullable disable

using AutoMapper;
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User
{
    public class FriendsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFriendStore<UserFriend, ApplicationUser> _userFriendStore;

        public FriendsModel(UserManager<ApplicationUser> userManager, IUserFriendStore<UserFriend, ApplicationUser> userFriendStore)
        {
            _userManager = userManager;
            _userFriendStore = userFriendStore;
        }

        [BindProperty]
        public IEnumerable<BasicUserInfoDTO> FindFriends { get; set; } = new List<BasicUserInfoDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
           
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            FindFriends = await _userFriendStore.GetUserFriendsAsync(user.Id);
            return Page();
        }
    }
}