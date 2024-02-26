#nullable disable

using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BoardGameBrawl.Areas.Identity.Pages.AppUser
{
    public class SendMessageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStore<MessageModel, ApplicationUser> _messageStore;

        public SendMessageModel(UserManager<ApplicationUser> userManager, IMessageStore<MessageModel, ApplicationUser> messageStore)
        {
            _userManager = userManager;
            _messageStore = messageStore;
        }

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }

        [BindProperty]
        public MessageInput Input { get; set; }

        public ApplicationUser ActiveUser { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class MessageInput
        {
            [Required]
            [Display(Name = "Sender")]
            public string Sender { get; set; }
            
            [Required]
            [Display(Name = "Receiver")]
            public string Receiver { get; set; }

            [Required]
            [MaxLength(250)]
            [Display(Name = "Message Title")]
            public string MessageTopic { get; set; }

            [Required]
            [MaxLength(500)]
            [Display(Name = "Message Body")]
            public string MessageBody { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ActiveUser = await _userManager.GetUserAsync(User);
            if (ActiveUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ApplicationUser = await _userManager.FindByNameAsync(UserName);
            if (ApplicationUser == null)
            {
                return NotFound($"User with that Username: {UserName} has not been found.");
            }

            Input = new MessageInput
            {
                Sender = ActiveUser.UserName,
                Receiver = ApplicationUser.UserName,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ActiveUser = await _userManager.GetUserAsync(User);
            ApplicationUser = await _userManager.FindByNameAsync(UserName);

            // creating new instance of MessageModel and setting all its fields by values provided on website
            MessageModel newMessage = CreateMessageModel();

            await _messageStore.SetSenderIdAsync(newMessage, ActiveUser.Id);
            await _messageStore.SetReceiverIdAsync(newMessage, ApplicationUser.Id);
            await _messageStore.SetMessageTitleAsync(newMessage, Input.MessageTopic);
            await _messageStore.SetMessageBodyAsync(newMessage, Input.MessageBody);
            await _messageStore.SetSenderAsync(newMessage, ActiveUser);
            await _messageStore.SetReceiverAsync(newMessage, ApplicationUser);

            DateTime messageSendingTime = DateTime.Now;
            await _messageStore.SetMessageSendingTimeAsync(newMessage, messageSendingTime);

            IdentityResult result = await _messageStore.CreateMessageAsync(newMessage);

            if (result.Succeeded)
            {
                StatusMessage = "Message has been sent successflly";
                return RedirectToPage();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                StatusMessage = "Error - message has not been sent. Try again";
                return Page();
            }
        }

        private MessageModel CreateMessageModel()
        {
            try
            {
                return Activator.CreateInstance<MessageModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(MessageModel)}'.");
            }
        }
    }
}