using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Dto;
using CustomerService.Entities;
using CustomerService.Messaging;
using CustomerService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMq.Shared.Messaging;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerCreatedMessagePublisher _createdMessagePublisher;
        private readonly CustomerDeletedMessagePublisher _deletedMessagePublisher;

        private readonly CustomerRepository _repository;
        
        public CustomerController(IOptions<RabbitMqConfiguration> rabbitMq, CustomerRepository repository)
        {
            _createdMessagePublisher = new CustomerCreatedMessagePublisher(rabbitMq);
            _deletedMessagePublisher = new CustomerDeletedMessagePublisher(rabbitMq);
            _repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            Customer customer = _repository.Get(id);

            if (customer == null)
            {
                return NotFound();
            }
            
            return Ok(customer);
        }
            
        [HttpGet("")]
        public ActionResult<IEnumerable<Customer>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpPost("")]
        public async Task<ActionResult<Customer>> Create(CreateCustomerInput input)
        {
            Customer customer = new ()
            {
                Name = input.Name
            };
            
            _repository.Create(customer);

            await _createdMessagePublisher.Send(customer);

            return customer;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            Customer customer = _repository.Get(id);
            
            await _deletedMessagePublisher.Send(customer);

            return customer;
        }
    }
}