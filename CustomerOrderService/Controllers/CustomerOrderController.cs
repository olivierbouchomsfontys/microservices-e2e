using System.Collections.Generic;
using CustomerOrderService.Dto;
using CustomerOrderService.Entities;
using CustomerOrderService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerOrderController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly OrderRepository _orderRepository;
        
        public CustomerOrderController(CustomerRepository customerRepository, OrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("{customerId}")]
        public ActionResult<CustomerOrderOutput> Get(int customerId)
        {
            Customer customer = _customerRepository.Get(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            IEnumerable<Order> orders = _orderRepository.GetForCustomer(customerId);

            CustomerOrderOutput output = new ()
            {
                Customer = customer,
                Orders = orders
            };

            return Ok(output);
        }
    }
}