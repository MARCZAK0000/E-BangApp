
using BackgrounMessageQueues.QueueComponents.Base;

namespace BackgrounMessageQueues.QueueComponents
{
    public class EmailQueueHandlerService : QueueHandlerService
    {
        public override string ToString()
        {
            return $"Current message in Queue : {_semaphore.CurrentCount}" ;
        }
    }
}
