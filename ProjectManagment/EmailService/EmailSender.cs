namespace ProjectManagment.EmailService
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using Microsoft.Extensions.Options;
    using System.Net.Mail;

    public interface IEmailSender
    {
        Task SendEmailAsync(string recipient, string subject, string body);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("project_mm", _emailSettings.SenderEmail));
                email.To.Add(new MailboxAddress("", recipient));
                email.Subject = subject;
                email.Body = new TextPart("plain") { Text = body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch
            {

            }
           
        }
    }
}
