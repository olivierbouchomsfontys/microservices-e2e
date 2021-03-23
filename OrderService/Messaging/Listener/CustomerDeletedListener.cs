using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Messaging.Model;
using RabbitMq.Shared.Messaging;

namespace OrderService.Messaging.Listener
{
    /// <summary>
    /// Listener that executes when a customer is deleted.
    /// </summary>
    public class CustomerDeletedListener : MessageListenerBase<CustomerDeletedModel>
    {
        protected override string Subject => "CustomerDeleted";
        
        private readonly ILogger<CustomerDeletedListener> _logger;

        public CustomerDeletedListener(IOptions<RabbitMqConfiguration> options, ILogger<CustomerDeletedListener> logger) : base(options)
        {
            _logger = logger;
        }
        
        protected override void HandleMessage(CustomerDeletedModel model)
        {
            _logger.LogInformation("{Nameof} {Model}", nameof(HandleMessage), model);
        }
    }
}