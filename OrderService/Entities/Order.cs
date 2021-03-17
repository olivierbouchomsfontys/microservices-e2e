using System;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int CustomerId { get; set; }
    }
}