using API.Pocker.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Pocker.Support
{
    public class AmqpService
    {
        private readonly AmqpSettings amqpInfo;
        private readonly ConnectionFactory connectionFactory;
        private const string QueueName = "AllVotes";

        private IModel _rabbitClient;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public AmqpService(IOptions<AmqpSettings> ampOptionsSnapshot)
        {
            amqpInfo = ampOptionsSnapshot.Value;

            connectionFactory = new ConnectionFactory
            {
                UserName = amqpInfo.Username,
                Password = amqpInfo.Password,
                VirtualHost = amqpInfo.VirtualHost,
                HostName = amqpInfo.HostName,
                Uri = new Uri(amqpInfo.Uri)
            };
        }

        private IModel BuildChannel()
        {
            _semaphore.Wait();
            try
            {
                if (_rabbitClient != null)
                {
                    return _rabbitClient;
                }

                var conn = connectionFactory.CreateConnection();
                _rabbitClient = conn.CreateModel();

                _rabbitClient.QueueDeclare(
                    queue: QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                return _rabbitClient;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void PublishMessage(object message)
        {
            var channel = BuildChannel();

            var jsonPayload = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonPayload);

            channel.BasicPublish(exchange: "",
                routingKey: QueueName,
                basicProperties: null,
                body: body
            );
        }
    }
}
