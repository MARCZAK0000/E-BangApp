using E_BangApplication.IQueueService;
using E_BangDomain.BackgroundTask;
using E_BangDomain.Repository;

namespace E_BangInfrastructure.Repository
{
    public class EmailSenderRepository : IEmailSenderRepository
    {
        private readonly IMessageTask _backgroundTask;
        private readonly IMessageSenderHandlerQueue _queue;

        public EmailSenderRepository(IMessageTask backgroundTask, IMessageSenderHandlerQueue queue)
        {
            _backgroundTask = backgroundTask;
            _queue = queue;
        }

        public Task<bool> SendEmailToQueueAsync()
        {
            throw new NotImplementedException();
        }
    }
}
