using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Dto;
using CustomerService.Entities;
using CustomerService.Messaging;
using CustomerService.Repository;
using Microsoft.AspNetCore.Mvc;
using RabbitMq.Shared.Rest.Errors;
using RabbitMq.Shared.Rest.Errors.Action;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly Lazy<CustomerCreatedMessagePublisher> _createdMessagePublisher;
        private readonly Lazy<CustomerDeletedMessagePublisher> _deletedMessagePublisher;

        private readonly CustomerRepository _repository;
        
        public CustomerController(CustomerRepository repository, Lazy<CustomerCreatedMessagePublisher> createdMessagePublisher, Lazy<CustomerDeletedMessagePublisher> deletedMessagePublisher)
        {
            _repository = repository;
            _createdMessagePublisher = createdMessagePublisher;
            _deletedMessagePublisher = deletedMessagePublisher;
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            Customer customer = _repository.Get(id);

            if (customer == null)
            {
                return NotFound(NotFoundResponse.Create<Customer>(id));
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

            await _createdMessagePublisher.Value.Send(customer);

            return customer;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            Customer customer = _repository.Get(id);

            if (customer == null)
            {
                return NotFound(NotFoundResponse.Create<Customer>(id, CrudAction.Delete));
            }

            _repository.Delete(id);
            
            await _deletedMessagePublisher.Value.Send(customer);

            return customer;
        }
    }
}