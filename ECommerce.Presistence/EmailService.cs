using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace ECommerce.Presistence
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _options;

        public EmailService(IOptions<EmailSettings> options)
        {
            _options = options;
        }
        public async Task SendEmailAsync(EmailDTO emailDTO)
        {
            // Combine Message and Settings to send email logic  => need to download pachages here
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Value.Email),
                Subject = emailDTO.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(emailDTO.To));
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            var builder = new BodyBuilder();
            builder.TextBody = emailDTO.Body;
            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_options.Value.Host, int.Parse(_options.Value.Port), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_options.Value.Email, _options.Value.Password);
            await smtp.SendAsync(mail);

            await smtp.DisconnectAsync(true);
        }
    }
}


