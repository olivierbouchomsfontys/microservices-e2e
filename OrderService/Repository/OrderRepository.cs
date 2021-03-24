using System.Collections.Generic;
using System.Linq;
using OrderService.Entities;

namespace OrderService.Repository
{
    public class OrderRepository
    {
        private readonly List<Order> _orders = new ();

        public Order Get(int id)
        {
            return _orders.Find(o => o.Id == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders.ToList();
        }

        public void Create(Order order)
        {
            lock (_orders)
            {
                order.AssignId(_orders.Count + 1);
                _orders.Add(order);
            }
        }

        public void DeleteForCustomer(int id)
        {
            _orders.RemoveAll(c => c.Id == id);
        }
    }
}