using System;
using System.Linq;
using System.Threading.Tasks;
using Pact_WebApi.Entities;
using Pact_WebApi.QueryParameters;

namespace Pact_WebApi.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        Task AddAsync(Customer customer);
        void Delete(Guid id);
        IQueryable<Customer> GetAll(CustomerQueryParameters customerQueryParameters);
        Customer GetSingle(Guid id);
        int Count();
        bool Save();
        void Update(Customer customer);
    }
}