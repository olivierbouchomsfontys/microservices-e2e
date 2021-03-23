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
        protected readonly RabbitMqConfiguration Configuration;

        protected abstract string Subject { get; }
        protected abstract string Queue { get; }

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

        protected IModel Channel { get; } 

        protected MessageListenerBase(IOptions<RabbitMqConfiguration> options)
        {
            Configuration = options.Value;

            Channel = Connection.CreateModel();
            Channel.QueueDeclare(Queue, true, false, false, null);
        }
        
        protected abstract void HandleMessage(TModel model);
        
        protected sealed override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new(Channel);

            consumer.Received += HandleMessage;

            Channel.BasicConsume(Configuration.QueueName, false, consumer);

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