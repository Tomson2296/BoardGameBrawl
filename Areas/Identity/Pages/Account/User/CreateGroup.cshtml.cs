#nullable disable
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.User
{
    public class CreateGroupModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGroupStore<GroupModel> _groupStore;
        private readonly IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> _groupParticipantStore;

        public CreateGroupModel(UserManager<ApplicationUser> userManager, IGroupStore<GroupModel> groupStore,
            IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser> groupParticipantStore)
        {
            _userManager = userManager;
            _groupStore = groupStore;
            _groupParticipantStore = groupParticipantStore;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(100)]
            [Display(Name = "Group Name")]
            public string GroupName { get; set; }

            [Display(Name = "Group Description")]
            [MaxLength(500)]
            public string GroupDesc { get; set; }

            [Display(Name = "Group Miniature")]
            public byte[] GroupMiniature { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Input = new InputModel
            {
                GroupName = string.Empty,
                GroupDesc = string.Empty,
                GroupMiniature = default
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Creating a new instance of group and saving it to database 
            GroupModel newGroup = CreateNewGroup();
            
            await _groupStore.SetGroupNameAsync(newGroup, Input.GroupName);

            if (Input.GroupDesc != null)
            {
                await _groupStore.SetGroupDescAsync(newGroup, Input.GroupDesc);
            }

            DateOnly creationTime = DateOnly.FromDateTime(DateTime.Now);
            await _groupStore.SetGroupCreationDateAsync(newGroup, creationTime);

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    // Upload the file if less than 2 MB
                    if (dataStream.Length < 2097152)
                    {
                        await file.CopyToAsync(dataStream);
                        Input.GroupMiniature = dataStream.ToArray();
                        await _groupStore.SetGroupMiniatureAsync(newGroup, Input.GroupMiniature);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }

            IdentityResult result = await _groupStore.CreateGroupAsync(newGroup, CancellationToken.None);
            if (result.Succeeded)
            {
                // If creation new group succeded -> create new instance od GroupParticipant
                // to make connection between user and newly created group
                GroupParticipant newGroupParticipant = CreateNewGroupParticipant();
                await _groupParticipantStore.SetGroupAsync(newGroupParticipant, newGroup);
                await _groupParticipantStore.SetUserAsync(newGroupParticipant, user);
                await _groupParticipantStore.SetOwnershipAsync(newGroupParticipant, true);
                await _groupParticipantStore.CreateGroupParticipantAsync(newGroupParticipant);

                StatusMessage = "Group has been created successflly";
                return RedirectToPage();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                StatusMessage = "Error - group has not been created. Try again";
                return Page();
            }
        }

        private GroupModel CreateNewGroup()
        {
            try
            {
                return Activator.CreateInstance<GroupModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Group)}'.");
            }
        }

        private GroupParticipant CreateNewGroupParticipant()
        {
            try
            {
                return Activator.CreateInstance<GroupParticipant>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(GroupParticipant)}'.");
            }
        }
    }
}