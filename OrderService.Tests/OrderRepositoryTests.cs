using System;
using OrderService.Entities;
using OrderService.Repository;
using Xunit;

namespace OrderService.Tests
{
    public class OrderRepositoryTests
    {
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _repository = new OrderRepository();
        }
        
        [Fact]
        public void TestCreate()
        {
            DateTime created = DateTime.Today;
            int customerId = 1;

            Order order = new()
            {
                CustomerId = customerId,
                Created = created
            };
            
            _repository.Create(order);
            
            Assert.True(order.Id != 0);
            Assert.True(order.CustomerId == customerId);
            Assert.True(order.Created == created);
        }

        [Fact]
        public void TestGetAll()
        {
            Assert.Empty(_repository.GetAll());
        }
    }
}