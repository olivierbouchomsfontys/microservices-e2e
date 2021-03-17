using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace RabbitMq.Shared.Messaging
{
    public abstract class MessagePublisherBase
    {
        protected readonly RabbitMqConfiguration Configuration;

        private const int DeliveryModePersistent = 2;
        
        protected abstract string Subject { get; }

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ()
                    {
                        HostName = Configuration.Hostname,
                        Port = Configuration.Port,
                        UserName = Configuration.UserName,
                        Password = Configuration.Password
                    };
                }

                return _connectionFactory;
            }
        }
        
        private IConnection Connection => ConnectionFactory.CreateConnection();

        protected MessagePublisherBase(IOptions<RabbitMqConfiguration> options)
        {
            Configuration = options.Value;
        }
        
        public void Send(object obj)
        {
            using (var channel = Connection.CreateModel())
            {
                IBasicProperties message = channel.CreateBasicProperties();

                message.ContentType = Configuration.ContentType;
                message.DeliveryMode = DeliveryModePersistent;

                message.Headers = GetHeaders(Subject); 

                byte[] body = JsonSerializer.SerializeToUtf8Bytes(obj);

                channel.BasicPublish(Configuration.Exchange, Configuration.QueueName, body: body);
            }
        }

        private IDictionary<string, object> GetHeaders(string subject)
        {
            return new Dictionary<string, object>
            {
                ["MessageType"] = subject
            };
        }
    }
}