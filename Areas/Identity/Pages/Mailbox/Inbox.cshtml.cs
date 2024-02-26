#nullable disable

using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Data.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using BoardGameBrawl.Services;

namespace BoardGameBrawl.Areas.Identity.Pages.Mailbox
{
    public class InboxModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStore<MessageModel, ApplicationUser> _messageStore;

        public InboxModel(UserManager<ApplicationUser> userManager, IMessageStore<MessageModel, ApplicationUser> messageStore)
        {
            _userManager = userManager;
            _messageStore = messageStore;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public IEnumerable<MessageModel> UserMessages_Receiver { get; set; } = new List<MessageModel>();

        [BindProperty]
        public IEnumerable<BasicUserInfoDTO> Senders_List { get; set; } = new List<BasicUserInfoDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser  = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Senders_List = await _messageStore.GetListofMessageSendersAsync(ApplicationUser.Id);
            UserMessages_Receiver = await _messageStore.GetMessagesByReceiverIdAsync(ApplicationUser.Id);
            return Page();
        }
    }
}