using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OrderService.Messaging.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq.Shared.Messaging;
using RabbitMq.Shared.Messaging.Extensions;

namespace OrderService.Messaging.Listener
{
    /// <summary>
    /// Listener that executes when a customer is deleted.
    /// </summary>
    public class CustomerDeletedListener : MessageListenerBase
    {
        protected override string Subject => "CustomerDeleted";
        protected override string Queue => "CustomerService";

        public CustomerDeletedListener(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }
        
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);

            consumer.Received += HandleMessage;

            Channel.BasicConsume(Configuration.QueueName, false, consumer);

            return Task.CompletedTask;
        }
        
        protected override void HandleMessage(object sender, BasicDeliverEventArgs args)
        {
            if (ShouldHandleMessage(args))
            {
                CustomerDeletedModel model = args.GetModel<CustomerDeletedModel>();

                Channel.BasicAck(args.DeliveryTag, false);
            }
            
            // TODO Handle
        }

    }
}