using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMq.Shared.Messaging.Extensions;

namespace RabbitMq.Shared.Messaging
{
    public abstract class MessagePublisherBase
    {
        private readonly RabbitMqConfiguration _configuration;

        protected abstract string Subject { get; }

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ConnectionFactory()
                    {
                        HostName = _configuration.Hostname,
                        Port = _configuration.Port,
                        UserName = _configuration.UserName,
                        Password = _configuration.Password
                    };
                }

                return _connectionFactory;
            }
        }
        
        private IConnection Connection => ConnectionFactory.CreateConnection();
        
        private IModel Channel { get; } 

        protected MessagePublisherBase(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
            
            Channel = Connection.CreateModel();
            Channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout);
        }
        
        public async Task Send(object obj)
        {
            IBasicProperties message = Channel.CreateBasicProperties();

            message.ContentType = _configuration.ContentType;
            message.SetSubject(Subject);

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(obj);

            await DoSend(message, body);
        }

        private async Task DoSend(IBasicProperties message, byte[] body)
        {
            await Task.Run(() =>
            {
                Channel.BasicPublish(_configuration.Exchange, string.Empty, message, body);
            });
        }
    }
}