using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderService.Dto;
using OrderService.Entities;
using OrderService.Messaging;
using OrderService.Repository;
using RabbitMq.Shared.Messaging;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly Lazy<OrderCreatedMessagePublisher> _createdMessagePublisher;
        private readonly OrderRepository _repository;

        public OrderController(Lazy<OrderCreatedMessagePublisher> createdMessagePublisher, OrderRepository repository)
        {
            _createdMessagePublisher = createdMessagePublisher;
            _repository = repository;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            Order order = _repository.Get(id);

            if (order == null)
            {
                return NotFound();
            }
            
            return Ok(order);
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpPost("")]
        public async Task<ActionResult<Order>> Create(CreateOrderInput input)
        {
            Order order = new()
            {
                Created = input.Created,
                CustomerId = input.CustomerId
            };
            
            _repository.Create(order);
            
            await _createdMessagePublisher.Value.Send(order);

            return order;
        }
    }
}