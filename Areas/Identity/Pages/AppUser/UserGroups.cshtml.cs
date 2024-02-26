#nullable disable
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.DTO;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.AppUser
{
    public class UserGroupsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> _groupParticipantStore;

        public UserGroupsModel(UserManager<ApplicationUser> userManager,
            IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> groupParticipantStore)
        {
            _userManager = userManager;
            _groupParticipantStore = groupParticipantStore;
        }

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public IEnumerable<GroupInfoDTO> Groups { get; set; } = new List<GroupInfoDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.FindByNameAsync(UserName);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Groups = await _groupParticipantStore.GetUserGroupsParticipationListAsync(ApplicationUser);
            return Page();
        }
    }
}