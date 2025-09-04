using E_BangEmailWorker.Model;
using MimeKit;
namespace E_BangEmailWorker.Repository
{
    public class MessageRepository : IMessageRepository
    {
        public MimeMessage BuildMessage(SendMailDto sendMailDto, CancellationToken token)
        {
            var message = new MimeMessage();
            message.From.Add(sendMailDto.AddressHost);
            message.To.Add(sendMailDto.AddressTo);
            message.Subject = sendMailDto.EmailSubject;
            var builder = new BodyBuilder
            {
               HtmlBody = sendMailDto.EmailBody
            };
            message.Body = builder.ToMessageBody();
            return message;
        }
    }
}
