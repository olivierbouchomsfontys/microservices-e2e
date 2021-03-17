using System.Collections.Generic;
using System.Linq;
using CustomerService.Entities;
using CustomerService.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private static readonly ICollection<Customer> Customers = new List<Customer>();
        private readonly CustomerMessagePublisher _messagePublisher;
        
        public CustomerController(IOptions<RabbitMqConfiguration> rabbitMq)
        {
            _messagePublisher = new CustomerMessagePublisher(rabbitMq);
        }

        [HttpGet("")]
        public ActionResult<Customer> Get(int id)
        {
            return Ok(Customers.First(c => c.Id == id));
        }
            
        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Customer>> GetAll()
        {
            return Ok(Customers);
        }

        [HttpPost("")]
        public ActionResult<Customer> Create(Customer customer)
        {
            customer.Id = Customers.Count;

            Customers.Add(customer);
            
            _messagePublisher.Send(customer);

            return customer;
        }
    }
}