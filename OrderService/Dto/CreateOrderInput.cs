using System;

namespace OrderService.Dto
{
    public class CreateOrderInput
    {
        public int CustomerId { get; set; }
        public DateTime Created { get; set; }
    }
}