using System.Runtime.InteropServices.ObjectiveC;

namespace E_BangEmailWorker.EmailMessageBuilderFactory
{
    public interface IEmailBuilderStrategy
    {
        public string EmailBuilderRoundRobin(object parameters);
    }
}
