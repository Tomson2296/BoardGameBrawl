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
    public class ViewMessageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStore<MessageModel, ApplicationUser> _messageStore;

        public ViewMessageModel(UserManager<ApplicationUser> userManager, IMessageStore<MessageModel, ApplicationUser> messageStore)
        {
            _userManager = userManager;
            _messageStore = messageStore;
        }

        [BindProperty(SupportsGet = true)]
        public string MessageId { get; set; }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public BasicUserInfoDTO Sender { get; set; }

        [BindProperty]
        public BasicUserInfoDTO Receiver { get; set; }

        [BindProperty]
        public MessageModel Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (ApplicationUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Message = await _messageStore.GetMessageByIdAsync(MessageId);
            Sender = await _messageStore.GetSenderInfoByMessageIdAsync(MessageId);
            Receiver = await _messageStore.GetReceiverInfoByMessageIdAsync(MessageId);

            return Page();
        }
    }
}
