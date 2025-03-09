using MimeKit;

namespace E_BangEmailWorker.Model
{
    public class SendMailDto
    {
        public MailboxAddress AddressHost { get; }
        public MailboxAddress AddressTo { get; }
        public EmailBody EmailBody { get; }

        public SendMailDto(MailboxAddress addressHost, MailboxAddress to, EmailBody emailBody)
        {
            AddressHost = addressHost;
            AddressTo = to;
            EmailBody = emailBody;
        }
    }
}