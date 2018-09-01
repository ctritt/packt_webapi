using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pact_WebApi.Entities;
using Pact_WebApi.QueryParameters;
using System.Linq.Dynamic.Core;

namespace Pact_WebApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PactDbContext _context;

        public CustomerRepository(PactDbContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAll(CustomerQueryParameters customerQueryParameters)
        {
            IQueryable<Customer> _allCustomers = _context.Customers;

            if (customerQueryParameters.HasQuery)
            {
                _allCustomers = _allCustomers.Where(x =>
                    x.FirstName.ToLowerInvariant().Contains(customerQueryParameters.Query.ToLowerInvariant()) ||
                    x.LastName.ToLowerInvariant().Contains(customerQueryParameters.Query.ToLowerInvariant()));
            }

            return _allCustomers
                .OrderBy(customerQueryParameters.OrderBy, customerQueryParameters.Descending)
                .Skip(customerQueryParameters.PageCount * (customerQueryParameters.Page - 1))
                .Take(customerQueryParameters.PageCount);
        }

        public Customer GetSingle(Guid id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public async Task AddAsync(Customer customer)
        {
           await _context.Customers.AddAsync(customer);
        }

        public void Delete(Guid id)
        {
            var customer = GetSingle(id);
            _context.Remove(customer);
        }

        public void Update(Customer customer)
        {
            _context.Update(customer);
        }

        public int Count()
        {
            return _context.Customers.Count();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
