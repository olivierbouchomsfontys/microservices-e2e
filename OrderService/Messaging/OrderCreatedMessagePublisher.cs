using Microsoft.Extensions.Options;
using OrderService.Entities;
using RabbitMq.Shared.Messaging;

namespace OrderService.Messaging
{
    public class OrderCreatedMessagePublisher : MessagePublisherBase<Order>
    {
        protected override string Subject => "OrderCreated";

        public OrderCreatedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }
    }
}