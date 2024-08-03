using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace CrmAPP.Services
{
    public class EmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "noreply.crm2025@gmail.com";
        private readonly string smtpPassword = "vmwh iwrd owgk jeez";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CRM System", smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUser, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
