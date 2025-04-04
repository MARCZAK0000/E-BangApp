using E_BangDomain.Repository;

namespace E_BangInfrastructure.Repository
{
    public class EmailSenderRepository : IEmailSenderRepository
    {
        public Task<bool> SendEmailToQueueAsync()
        {
            throw new NotImplementedException();
        }
    }
}
