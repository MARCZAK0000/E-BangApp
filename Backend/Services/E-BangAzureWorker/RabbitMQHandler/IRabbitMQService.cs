using RabbitMQ.Client;

namespace E_BangAzureWorker.RabbitMQHandler
{
    interface IRabbitMQService
    {
        IChannel Channel { get;  }

        IConnection Connection { get;  }
        Task InitilizeQueue();
        Task InvokeQueue(Func<byte[], Task> handleMessage);
        Task DisposeQueue();
    }
}
