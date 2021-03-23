using CustomerOrderService.Entities;
using CustomerOrderService.Messaging.Model;
using CustomerOrderService.Repository;
using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerOrderService.Messaging.Listener
{
    public class OrderCreatedListener : MessageListenerBase<OrderCreatedModel>
    {
        private readonly OrderRepository _repository;
        
        protected override string Subject => "OrderCreated";

        public OrderCreatedListener(IOptions<RabbitMqConfiguration> options, OrderRepository repository) : base(options)
        {
            _repository = repository;
        }

        protected override void HandleMessage(OrderCreatedModel model)
        {
            Order order = new()
            {
                Id = model.Id,
                Created = model.Created,
                CustomerId = model.CustomerId
            };

            _repository.Add(order);
        }
    }
}