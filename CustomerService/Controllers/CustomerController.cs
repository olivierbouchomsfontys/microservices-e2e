using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.Dto;
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

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            Customer customer = Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }
            
            return Ok(customer);
        }
            
        [HttpGet("")]
        public ActionResult<IEnumerable<Customer>> GetAll()
        {
            return Ok(Customers);
        }

        [HttpPost("")]
        public async Task<ActionResult<Customer>> Create(CreateCustomerInput input)
        {
            Customer customer = new ()
            {
                Name = input.Name,
                Id = Customers.Count + 1
            };

            Customers.Add(customer);
            
            await _createdMessagePublisher.Send(customer);

            return customer;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            Customer customer = Customers.FirstOrDefault(c => c.Id == id);
            
            Customers.Remove(customer);
            
            await _deletedMessagePublisher.Send(customer);

            return customer;
        }
    }
}