#nullable disable

using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Data.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoardGameBrawl.Services;

namespace BoardGameBrawl.Areas.Identity.Pages.Group
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGroupStore<GroupModel> _groupStore;
        private readonly IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> _groupParticipantStore;

        public IndexModel(UserManager<ApplicationUser> userManager,
            IGroupStore<GroupModel> groupStore,
            IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> groupParticipantStore)
        {
            _userManager = userManager;
            _groupStore = groupStore;
            _groupParticipantStore = groupParticipantStore;
        }

        [BindProperty(SupportsGet = true)]
        public string GroupName { get; set; } 

        [BindProperty]
        public GroupModel Group { get; set; }

        [BindProperty]
        public IEnumerable<BasicUserInfoDTO> Group_Participants { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Group = await _groupStore.FindGroupByGroupNameAsync(GroupName);
            Group_Participants = await _groupParticipantStore.GetGroupParticipantsListAsync(Group); 
            return Page();
        }
    }
}