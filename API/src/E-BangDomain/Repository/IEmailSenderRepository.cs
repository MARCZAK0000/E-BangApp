namespace E_BangDomain.Repository
{
    public interface IEmailSenderRepository
    {
        Task <bool> SendEmailToQueueAsync();

    }
}
