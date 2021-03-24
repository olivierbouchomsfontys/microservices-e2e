using System.Collections.Generic;
using System.Linq;
using CustomerService.Entities;

namespace CustomerService.Repository
{
    public class CustomerRepository
    {
        private readonly List<Customer> _customers = new ();

        public Customer Get(int id)
        {
            return _customers.Find(c => c.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customers.ToList();
        }

        public void Create(Customer customer)
        {
            lock (_customers)
            {
                customer.AssignId(_customers.Count + 1);
                _customers.Add(customer);
            }
        }
    }
}