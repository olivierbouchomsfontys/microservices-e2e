using CustomerService.Entities;
using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerService.Messaging
{
    public class CustomerCreatedMessagePublisher : MessagePublisherBase<Customer>
    {
        protected override string Subject => "CustomerCreated";

        public CustomerCreatedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
            
        }
    }
}