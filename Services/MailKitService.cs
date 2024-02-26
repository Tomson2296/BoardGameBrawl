using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Humanizer;

namespace BoardGameBrawl.Services
{
    public class MailKitService : IMailKitService
    {
        private readonly IConfiguration _configuration;

        public MailKitService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            try
            {
                // Using saved EmailConfiguration
                var username = _configuration.GetSection("EmailConfiguration:Username").Value;
                var smtp = _configuration.GetSection("EmailConfiguration:Smtp").Value;
                bool portParsed = int.TryParse(_configuration.GetSection("EmailConfiguration:Port").Value, out int port);
                var from = _configuration.GetSection("EmailConfiguration:From").Value;
                var password = _configuration.GetSection("EmailConfiguration:Password").Value;

                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

                // send email using OAuth 2.0 Google credentials
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, port, SecureSocketOptions.StartTls);
                    client.Authenticate(username, password);
                    client.Send(email);
                    client.Disconnect(true);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
