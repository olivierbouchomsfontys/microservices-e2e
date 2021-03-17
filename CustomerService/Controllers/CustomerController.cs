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
        
        private readonly CustomerCreatedMessagePublisher _createdMessagePublisher;
        private readonly CustomerDeletedMessagePublisher _deletedMessagePublisher;
        
        public CustomerController(IOptions<RabbitMqConfiguration> rabbitMq)
        {
            _createdMessagePublisher = new CustomerCreatedMessagePublisher(rabbitMq);
            _deletedMessagePublisher = new CustomerDeletedMessagePublisher(rabbitMq);
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
            
            _createdMessagePublisher.Send(customer);

            return customer;
        }

        [HttpDelete("")]
        public ActionResult<Customer> Delete(int id)
        {
            Customer customer = Customers.FirstOrDefault(c => c.Id == id);
            
            Customers.Remove(customer);
            
            _deletedMessagePublisher.Send(customer);

            return customer;
        }
    }
}