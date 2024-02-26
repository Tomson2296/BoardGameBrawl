#nullable disable
using AutoMapper;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class LockoutsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public IEnumerable<LockoutUserDTO> LockedOutUsers;
        public IEnumerable<LockoutUserDTO> NonLockedUsers;

        public LockoutsModel(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
       
        public int ListSize { get; set; } = 20;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } 

        public int Prev_Page { get; set; }

        public int Next_Page { get; set; }

        public async Task<TimeSpan> TimeLeft(LockoutUserDTO user)
        {
            var appUser = _mapper.Map<ApplicationUser>(user);
            return (await _userManager.GetLockoutEndDateAsync(appUser)).GetValueOrDefault().Subtract(DateTimeOffset.Now);
        }

        public async Task<IActionResult> OnGetAsync()
        {

            LockedOutUsers = await _userManager.Users.Where(user => user.LockoutEnd.HasValue
                    && user.LockoutEnd.Value > DateTimeOffset.Now).Select(u => new LockoutUserDTO { DTOUserID = u.Id, DTOUsername = u.UserName ,LockOutEnd = u.LockoutEnd })
                .OrderBy(u => u.DTOUsername).ToListAsync();

            NonLockedUsers = await _userManager.Users.Where(user => !user.LockoutEnd.HasValue
                    || user.LockoutEnd.Value <= DateTimeOffset.Now).Select(u => new LockoutUserDTO { DTOUserID = u.Id, DTOUsername = u.UserName, LockOutEnd = u.LockoutEnd })
                .OrderBy(u => u.DTOUsername).Skip(ListSize * (PageNumber - 1)).Take(ListSize).ToListAsync();
            
            Prev_Page = (PageNumber - 1) < 1 ? 1 : PageNumber - 1;
            Next_Page = PageNumber + 1;
            return Page();
        }

        public async Task<IActionResult> OnPostUnlockAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            await _userManager.SetLockoutEnabledAsync(user, false);
            return RedirectToPage("Lockouts", new { PageNumber });
        }

        // Locking out the user from the appplication for the next 24 hours
        // Updating security stamp -> otherwise user will still can sign up into aplicattion (immediate sign-up pitfall)
        public async Task<IActionResult> OnPostLockAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddHours(24));
            await _userManager.UpdateSecurityStampAsync(user);
            return RedirectToPage("Lockouts", new { PageNumber });
        }
    }
}