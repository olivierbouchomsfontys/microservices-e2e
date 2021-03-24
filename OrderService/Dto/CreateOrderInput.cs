using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace OrderService.Dto
{
    public class CreateOrderInput
    {
        public int CustomerId { get; init; }
        public DateTime Created { get; init; }
    }
}