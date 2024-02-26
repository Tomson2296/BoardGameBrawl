using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class DashboardModel : PageModel
    {
        public UserManager<ApplicationUser> _userManager;

        public DashboardModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public int UsersCount { get; set; } = 0;

        public int UsersUnconfirmed { get; set; } = 0;

        public int UsersLockedout { get; set; } = 0;

        public void OnGet()
        {
            UsersCount = _userManager.Users.Count();

            UsersUnconfirmed = _userManager.Users.Where(u => u.EmailConfirmed == false).Count();

            UsersLockedout = _userManager.Users.Where(u => u.AccessFailedCount == 5).Count();
        }
    }
}
