using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerService.Messaging
{
    public class CustomerCreatedMessagePublisher : MessagePublisherBase
    {
        protected override string Subject => "CustomerCreated";

        public CustomerCreatedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
            
        }
    }
}