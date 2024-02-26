#nullable disable
using AutoMapper;
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class UsersInRoleModel : PageModel
    {
        public UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsersInRoleModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<ViewUserDTO> UsersInRoles { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string ChosenRole { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; }

        public int PageSize { get; set; } = 20;

        public int TotalUsersNumber { get; set; }

        public int PreviousNumber { get; set; }

        public int NextNumber { get; set; }

        private async Task SetUserList(int batchSize, int batchNumber)
        {
            TotalUsersNumber = (await _userManager.GetUsersInRoleAsync(ChosenRole)).Count;
            UsersInRoles = _mapper.Map<List<ViewUserDTO>>(await _userManager.GetBatchOfUsersWithTheSameRole(batchSize, batchNumber, ChosenRole));
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetUserList(PageSize, PageNumber);
            PreviousNumber = (PageNumber - 1 < 1) ? 1 : PageNumber - 1;
            NextNumber = PageNumber + 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveUserFromRoleAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, ChosenRole);

            if (result.Succeeded)
            {
                StatusMessage = "User removed from role successfully.";
                await SetUserList(PageSize, PageNumber);
                return RedirectToPage("UsersInRole", new { ChosenRole, PageNumber });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    StatusMessage = "Error - user has not been removed from role. Tryagain.";
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }
            return Page();
        }
    }
}
