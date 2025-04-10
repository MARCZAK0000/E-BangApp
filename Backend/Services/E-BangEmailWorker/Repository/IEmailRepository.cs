using E_BangEmailWorker.Model;
using MimeKit;

namespace E_BangEmailWorker.Repository
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailAsync(MimeMessage mimeMessage, CancellationToken token);
    }
}
