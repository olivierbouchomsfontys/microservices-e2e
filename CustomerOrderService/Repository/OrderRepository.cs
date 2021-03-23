using System.Collections.Generic;
using System.Linq;
using CustomerOrderService.Entities;

namespace CustomerOrderService.Repository
{
    public class OrderRepository
    {
        private readonly List<Order> _orders = new ();

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public IEnumerable<Order> GetForCustomer(int customerId)
        {
            return _orders.Where(c => c.CustomerId == customerId);
        }
    }
}