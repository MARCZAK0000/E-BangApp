using E_BangEmailWorker.Model;
using MimeKit;

namespace E_BangEmailWorker.Repository
{
    public interface IMessageRepository
    {
        MimeMessage BuildMessage(SendMailDto sendMailDto, CancellationToken token);
        string GenerateMessage(EmailBody body, CancellationToken token);
    }
}
