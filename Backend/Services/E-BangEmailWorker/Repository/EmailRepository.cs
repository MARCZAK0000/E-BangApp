using E_BangEmailWorker.Model;
using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Threading;

namespace E_BangEmailWorker.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailConnectionOptions _emailConnectionOptions;
        private readonly ILogger<EmailServices> _logger;

        public EmailRepository(EmailConnectionOptions emailConnectionOptions, ILogger<EmailServices> logger)
        {
            _emailConnectionOptions = emailConnectionOptions;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(MimeMessage messageToSend, CancellationToken token)
        {
            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailConnectionOptions.SmptHost, _emailConnectionOptions.Port, MailKit.Security.SecureSocketOptions.StartTls, token);
                await smtp.AuthenticateAsync(new NetworkCredential(_emailConnectionOptions.EmailName, _emailConnectionOptions.Password), token);
                await smtp.SendAsync(messageToSend, token);
                await smtp.DisconnectAsync(true, token);
                _logger.LogInformation("Email Send to: {addressTo}", messageToSend.To);
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Email Not Send to: {addressTo}", messageToSend.To);
                throw;
            }
        }
    }
}
