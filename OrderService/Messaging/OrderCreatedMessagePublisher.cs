using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace OrderService.Messaging
{
    public class OrderCreatedMessagePublisher : MessagePublisherBase
    {
        protected override string Subject => "OrderCreated";

        public OrderCreatedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }
    }
}