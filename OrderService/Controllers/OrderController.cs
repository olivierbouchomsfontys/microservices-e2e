using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderService.Entities;
using OrderService.Messaging;
using RabbitMq.Shared.Messaging;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly ICollection<Order> Orders = new List<Order>();
        private readonly OrderMessagePublisher _messagePublisher;

        public OrderController(IOptions<RabbitMqConfiguration> rabbitMq)
        {
            _messagePublisher = new OrderMessagePublisher(rabbitMq);
        }
        
        [HttpGet("")]
        public ActionResult<Order> Get(int id)
        {
            return Ok(Orders.First(c => c.Id == id));
        }
            
        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            return Ok(Orders);
        }

        [HttpPost("")]
        public ActionResult<Order> Create(Order order)
        {
            order.Id = Orders.Count;

            Orders.Add(order);
            
            _messagePublisher.Send(order);

            return order;
        }
    }
}