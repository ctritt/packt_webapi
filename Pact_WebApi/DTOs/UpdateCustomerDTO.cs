using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pact_WebApi.DTOs
{
    public class UpdateCustomerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
