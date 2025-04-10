using E_BangEmailWorker.Model;
using E_BangEmailWorker.OptionsPattern;
using MailKit.Net.Smtp;
using System.Net;

namespace E_BangEmailWorker.Services
{
    [Obsolete]
    public class EmailServices : IEmailServices
    {
        public EmailServices(EmailConnectionOptions emailConnectionOptions, ILogger<EmailServices> logger)
        {
            _emailConnectionOptions = emailConnectionOptions;
            _logger = logger;
        }
        private readonly EmailConnectionOptions _emailConnectionOptions;
        private readonly ILogger<EmailServices> _logger;
        public override async Task<bool> SendMailAsync(SendMailDto send, CancellationToken cancellationToken)
        {
            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailConnectionOptions.SmptHost, _emailConnectionOptions.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
                await smtp.AuthenticateAsync(new NetworkCredential(_emailConnectionOptions.EmailName, _emailConnectionOptions.Password), cancellationToken);
                var message = GenerateDefaultMessage(send);
                await smtp.SendAsync(message, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
                _logger.LogInformation("Email Send to: {0}", send.AddressTo);
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Email Not Send to: {0}", send.AddressTo);
                throw;
            }
            
        }
    }
}