using E_BangEmailWorker.Model;
using E_BangEmailWorker.OptionsPattern;
using MailKit.Net.Smtp;
using System.Net;

namespace E_BangEmailWorker.Services
{
    public class EmailServices : IEmailServices
    {
        public EmailServices(EmailConnectionOptions emailConnectionOptions)
        {
            _emailConnectionOptions = emailConnectionOptions;
        }
        private readonly EmailConnectionOptions _emailConnectionOptions;
        public override async Task<bool> SendMailAsync(SendMailDto send, CancellationToken cancellationToken)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailConnectionOptions.SmptHost, _emailConnectionOptions.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(new NetworkCredential());
            throw new NotImplementedException();
        }
    }
}