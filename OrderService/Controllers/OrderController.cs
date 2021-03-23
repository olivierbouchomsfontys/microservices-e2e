using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderService.Dto;
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
        private readonly OrderCreatedMessagePublisher _createdMessagePublisher;

        public OrderController(IOptions<RabbitMqConfiguration> rabbitMq)
        {
            _createdMessagePublisher = new OrderCreatedMessagePublisher(rabbitMq);
        }
        
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            Order order = Orders.FirstOrDefault(c => c.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            
            return Ok(order);
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            return Ok(Orders);
        }

        [HttpGet("Customer/{customerId}")]
        public ActionResult<IEnumerable<Order>> GetForCustomer(int customerId)
        {
            return Ok(Orders.Where(c => c.CustomerId == customerId));
        }

        [HttpPost("")]
        public async Task<ActionResult<Order>> Create(CreateOrderInput input)
        {
            Order order = new()
            {
                Id = Orders.Count,
                Created = input.Created,
                CustomerId = input.CustomerId
            };
            
            Orders.Add(order);
            
            await _createdMessagePublisher.Send(order);

            return order;
        }
    }
}