using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace E_BangAzureWorker.RabbitMQHandler
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;

        private readonly 
        public IChannel Channel { get; private set; }
        public IConnection Connection { get; private set; }
        public async Task DisposeQueue()
        {
            await Connection.DisposeAsync();
            await Channel.DisposeAsync();
        }

        public async Task InitilizeQueue()
        {
            var factory = new ConnectionFactory()
            {
                HostName = ""
            };
            Connection = await factory.CreateConnectionAsync();
            Channel = await Connection.CreateChannelAsync();
           
        }

        public async Task InvokeQueue(Func<byte[], Task> handleMessage)
        {
            var eventConsumer = new AsyncEventingBasicConsumer(Channel);
            eventConsumer.ReceivedAsync += async (model, ea) =>
            {
                var message = ea.Body.ToArray();
                await handleMessage(message);
            };
            await Channel.BasicConsumeAsync("hello", autoAck: true, consumer: eventConsumer);
        }
    }
}
