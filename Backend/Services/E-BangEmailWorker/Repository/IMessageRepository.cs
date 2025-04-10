using E_BangEmailWorker.Model;
using MimeKit;

namespace E_BangEmailWorker.Repository
{
    public interface IMessageRepository
    {
        Task<string> GenerateMessage(RabbitMessageDto parameters);
        Task<MimeMessage> BuildMessage(SendMailDto sendMailDto);

    }
}
