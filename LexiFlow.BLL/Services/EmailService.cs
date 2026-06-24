using LexiFlow.BLL.Models.Emails;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;


namespace LexiFlow.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            var subject = "Your OTP Code - LexiFlow";

            var body = $@"
            <div style='font-family:Arial;padding:20px'>
                <h2>LexiFlow OTP Verification</h2>
                <p>Your OTP code is:</p>
                <h1 style='color:#4CAF50'>{otp}</h1>
                <p>This code will expire in 10 minutes.</p>
            </div>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_settings.FromName, _settings.UserName));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _settings.UserName,
                _settings.Password
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}

