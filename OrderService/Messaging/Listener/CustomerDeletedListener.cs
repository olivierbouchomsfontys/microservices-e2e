using Microsoft.Extensions.Options;
using OrderService.Messaging.Model;
using OrderService.Repository;
using RabbitMq.Shared.Messaging;

namespace OrderService.Messaging.Listener
{
    /// <summary>
    /// Listener that executes when a customer is deleted.
    /// </summary>
    public class CustomerDeletedListener : MessageListenerBase<CustomerDeletedModel>
    {
        protected override string Subject => "CustomerDeleted";

        private readonly OrderRepository _repository;

        public CustomerDeletedListener(IOptions<RabbitMqConfiguration> options, OrderRepository repository) : base(options)
        {
            _repository = repository;
        }
        
        protected override void HandleMessage(CustomerDeletedModel model)
        {
            _repository.DeleteForCustomer(model.Id);
        }
    }
}