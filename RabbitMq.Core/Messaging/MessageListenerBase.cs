using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq.Shared.Messaging.Extensions;

namespace RabbitMq.Shared.Messaging
{
    public abstract class MessageListenerBase<TModel> : BackgroundService where TModel : class
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

        protected MessageListenerBase(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;

            Channel = Connection.CreateModel();
            Channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout);
            
            QueueDeclareOk result = Channel.QueueDeclare(string.Empty, exclusive: true);

            Channel.QueueBind(result.QueueName, _configuration.Exchange, string.Empty);
        }
        
        protected abstract void HandleMessage(TModel model);
        
        protected sealed override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new(Channel);

            consumer.Received += HandleMessage;

            Channel.BasicConsume(string.Empty, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(object sender, BasicDeliverEventArgs args)
        {
            if (ShouldHandleMessage(args))
            {
                TModel model = args.GetModel<TModel>();
                
                HandleMessage(model);
                
                Channel.BasicAck(args.DeliveryTag, false);
            }
        }

        private bool ShouldHandleMessage(BasicDeliverEventArgs args)
        {
            return args.GetSubject()?.Equals(Subject, StringComparison.OrdinalIgnoreCase)
                ?? false;
        }
    }
}