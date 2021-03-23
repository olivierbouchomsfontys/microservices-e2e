using System.Collections.Generic;
using CustomerOrderService.Entities;

namespace CustomerOrderService.Repository
{
    public class CustomerRepository
    {
        private readonly Dictionary<int, Customer> _customers = new ();

        public void Add(Customer customer)
        {
            _customers.TryAdd(customer.Id, customer);
        }

        public void Delete(int id)
        {
            _customers.Remove(id);
        }

        public Customer Get(int id)
        {
            if (_customers.TryGetValue(id, out Customer customer))
            {
                return customer;
            }

            return null;
        }
    }
}