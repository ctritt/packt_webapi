using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pact_WebApi.DTOs;
using Pact_WebApi.QueryParameters;
using Pact_WebApi.Repositories;

namespace Pact_WebApi.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomersV2Controller : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersV2Controller(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        //[ProducesResponseType(typeof(IList<Customer>), 200)]
        public IActionResult GetAllCustomers([FromQuery] CustomerQueryParameters customerQueryParameters)
        {
            var customerEntities = _customerRepository.GetAll(customerQueryParameters);
            var customerDtos = customerEntities.Select(c => Mapper.Map<CustomerDTO>(c));

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { totalCount = _customerRepository.Count() }));

            return Ok(customerDtos);
        }
    }
}