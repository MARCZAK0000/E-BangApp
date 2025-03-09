using MimeKit;

namespace E_BangEmailWorker.Model
{
    public class SendMailDto
    {
        public int Id { get; set; }
        public MailboxAddress AddressHost { get; }
        public MailboxAddress AddressTo { get; }
        public EmailBody EmailBody { get; }

        public SendMailDto(int id,MailboxAddress addressHost, MailboxAddress to, EmailBody emailBody)
        {
            Id = id;
            AddressHost = addressHost;
            AddressTo = to;
            EmailBody = emailBody;
        }
    }
}