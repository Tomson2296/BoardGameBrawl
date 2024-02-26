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
    public class OutboxModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStore<MessageModel, ApplicationUser> _messageStore;

        public OutboxModel(UserManager<ApplicationUser> userManager, IMessageStore<MessageModel, ApplicationUser> messageStore) 
        {
            _userManager = userManager;
            _messageStore = messageStore;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public IEnumerable<MessageModel> UserMessages_Sender { get; set; }

        [BindProperty]
        public IEnumerable<BasicUserInfoDTO> Receivers_List { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Receivers_List = await _messageStore.GetListofMessageReceiversAsync(ApplicationUser.Id);
            UserMessages_Sender = await _messageStore.GetMessagesBySenderIdAsync(ApplicationUser.Id);
            return Page();
        }
    }
}