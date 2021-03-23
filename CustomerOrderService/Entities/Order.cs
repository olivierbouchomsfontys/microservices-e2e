using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CustomerOrderService.Entities
{
    public class Order
    {
        public int Id { get; init; }
        public DateTime Created { get; init; }
        public int CustomerId { get; init; }
    }
}