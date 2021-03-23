using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CustomerOrderService.Messaging.Model
{
    public record OrderCreatedModel
    {
        public int Id { get; init; }
        public DateTime Created { get; init; }
        public int CustomerId { get; init; }
    }
}