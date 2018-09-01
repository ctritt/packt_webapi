using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pact_WebApi.Entities
{
    public class PactDbContext : DbContext
    {
        public PactDbContext(DbContextOptions<PactDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
