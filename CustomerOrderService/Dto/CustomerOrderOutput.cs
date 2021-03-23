using System.Collections.Generic;
using CustomerOrderService.Entities;

namespace CustomerOrderService.Dto
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class CustomerOrderOutput
    {
        public Customer Customer { get; init; }
        public IEnumerable<Order> Orders { get; init; } = new List<Order>();
    }
}