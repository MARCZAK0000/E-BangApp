using E_BangEmailWorker.Model;
using MimeKit;
namespace E_BangEmailWorker.Repository
{
    public class MessageRepository : IMessageRepository
    {

        public Task<MimeMessage> BuildMessage(SendMailDto sendMailDto)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateMessage(RabbitMessageDto parameters)
        {
            throw new NotImplementedException();
        }
    }
}
