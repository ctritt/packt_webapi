using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pact_WebApi.Entities;

namespace Pact_WebApi.Services
{
    public class SeedDataService : ISeedDataService
    {
        private readonly PactDbContext _context;

        public SeedDataService(PactDbContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            _context.Database.EnsureCreated();
            _context.Customers.RemoveRange(_context.Customers);
            _context.SaveChanges();

            _context.AddRange(new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Casey",
                    LastName = "Tritt",
                    Age = 39
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Erika",
                    LastName = "Miller",
                    Age = 23
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
