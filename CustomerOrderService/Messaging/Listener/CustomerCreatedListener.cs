using CustomerOrderService.Entities;
using CustomerOrderService.Messaging.Model;
using CustomerOrderService.Repository;
using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerOrderService.Messaging.Listener
{
    public class CustomerCreatedListener : MessageListenerBase<CustomerCreatedModel>
    {
        private readonly CustomerRepository _repository;

        protected override string Subject => "CustomerCreated";

        public CustomerCreatedListener(IOptions<RabbitMqConfiguration> options, CustomerRepository repository) : base(options)
        {
            _repository = repository;
        }

        protected override void HandleMessage(CustomerCreatedModel model)
        {
            Customer customer = new()
            {
                Id = model.Id,
                Name = model.Name
            };

            _repository.Add(customer);
        }
    }
}