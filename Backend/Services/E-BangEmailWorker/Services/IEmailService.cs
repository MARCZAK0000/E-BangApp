using E_BangEmailWorker.Model;
using MimeKit;

[Obsolete]
public abstract class IEmailServices {
    public IEmailServices()
    {

    }

    public abstract Task<bool> SendMailAsync(SendMailDto send, CancellationToken token);
    
    protected virtual MimeMessage GenerateDefaultMessage(SendMailDto send) 
    {
        var message = new MimeMessage();
        message.From.Add(send.AddressHost);
        message.To.Add(send.AddressTo);
        message.Subject =  send.EmailBody.subject;
        var builder = new BodyBuilder
        {
            HtmlBody = send.EmailBody.body
        };
        message.Body = builder.ToMessageBody();
        return message;
    }

}