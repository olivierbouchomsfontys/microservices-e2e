using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerService.Messaging
{
    public class CustomerDeletedMessagePublisher : MessagePublisherBase
    {
        protected override string Subject => "CustomerDeleted";

        public CustomerDeletedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }
    }
}