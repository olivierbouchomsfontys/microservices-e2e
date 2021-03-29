using System;
using System.Collections.Generic;
using System.Linq;
using OrderService.Entities;
using OrderService.Repository;
using Xunit;

namespace OrderService.Tests
{
    public class OrderRepositoryIntegrationTests
    {
        private readonly OrderRepository _repository;

        public OrderRepositoryIntegrationTests()
        {
            _repository = new OrderRepository();
        }

        [Fact]
        public void TestCreateAndGet()
        {
            DateTime created = DateTime.Today;
            int customerId = 1;

            Order order = new()
            {
                CustomerId = customerId,
                Created = created
            };
            
            _repository.Create(order);

            Order retrieved = _repository.Get(order.Id);
            
            Assert.NotNull(retrieved);
            Assert.True(retrieved.CustomerId == customerId);
            Assert.True(retrieved.Created == created);
        }

        [Fact]
        public void TestCreateAndGetAll()
        {
            DateTime created = DateTime.Today;
            int customerId = 1;

            Order order = new()
            {
                CustomerId = customerId,
                Created = created
            };
            
            _repository.Create(order);

            IEnumerable<Order> orders = _repository.GetAll();
            
            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
            Assert.True(orders.Count() == 1);

            Order retrieved = orders.First();
            
            Assert.NotNull(retrieved);
            Assert.True(retrieved.CustomerId == customerId);
            Assert.True(retrieved.Created == created);
        }

        [Fact]
        public void TestCreateAndDeleteForCustomer()
        {
            int customerId = 1;

            Order order = new()
            {
                CustomerId = customerId,
                Created = DateTime.Now
            };
            
            _repository.Create(order);
            
            _repository.DeleteForCustomer(customerId);

            IEnumerable<Order> orders = _repository.GetAll();
            
            Assert.Empty(orders);
        }
    }
}