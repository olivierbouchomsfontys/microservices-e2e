using CustomerOrderService.Messaging.Model;
using CustomerOrderService.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerOrderService.Messaging.Listener
{
    public class CustomerDeletedListener : MessageListenerBase<CustomerDeletedModel>
    {
        private readonly CustomerRepository _repository;
        
        protected override string Subject => "CustomerDeleted";

        public CustomerDeletedListener(IOptions<RabbitMqConfiguration> options, CustomerRepository repository, ILogger<CustomerDeletedListener> logger) : base(options, logger)
        {
            _repository = repository;
        }
        
        protected override void HandleMessage(CustomerDeletedModel model)
        {
            _repository.Delete(model.Id);
        }
    }
}