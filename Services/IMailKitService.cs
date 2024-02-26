using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BoardGameBrawl.Services
{
    public interface IMailKitService
    {
        public Task SendEmailAsync(string to, string subject, string htmlMessage);
    }
}
