using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMq.Shared.Messaging.Extensions;

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
            using (IModel channel = Connection.CreateModel())
            {
                IBasicProperties message = channel.CreateBasicProperties();

                message.ContentType = Configuration.ContentType;
                message.DeliveryMode = DeliveryModePersistent;
                message.SetSubject(Subject);

                byte[] body = JsonSerializer.SerializeToUtf8Bytes(obj);
                
                channel.BasicPublish(Configuration.Exchange, Configuration.QueueName, message, body);
            }
        }
    }
}