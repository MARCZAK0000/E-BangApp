using MimeKit;

namespace E_BangEmailWorker.Model
{
    public class SendMailDto
    {
       public MailboxAddress AddressHost { get; }
        public MailboxAddress AddressTo { get; }
        public string EmailBody { get; }

        public string EmailSubject { get; }

        public SendMailDto(string to, string emailBody, string emailSubject)
        {
            AddressHost = new MailboxAddress("host", "ebangapp1998@gmail.com");
            AddressTo = new MailboxAddress("client", to);
            EmailBody = emailBody;
            EmailSubject = emailSubject;
        }
    }
}