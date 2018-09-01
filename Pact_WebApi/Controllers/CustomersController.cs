using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pact_WebApi.DTOs;
using Pact_WebApi.Entities;
using Pact_WebApi.QueryParameters;
using Pact_WebApi.Repositories;

namespace Pact_WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        //[ProducesResponseType(typeof(IList<Customer>), 200)]
        public IActionResult GetAllCustomers([FromQuery] CustomerQueryParameters customerQueryParameters)
        {
            var customerEntities = _customerRepository.GetAll(customerQueryParameters);
            var customerDtos = customerEntities.Select(c => Mapper.Map<CustomerDTO>(c));

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new {totalCount = _customerRepository.Count()}));

            return Ok(customerDtos);
        }

        [HttpGet("{id}", Name = "GetSingleCustomer")]
        public IActionResult GetSingleCustomer(Guid id)
        {
            var customerEntity = _customerRepository.GetSingle(id);

            if (customerEntity == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CustomerDTO>(customerEntity));
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDTO customer)
        {
            var customerEntity = Mapper.Map<Customer>(customer);
            await _customerRepository.AddAsync(customerEntity);
            if (_customerRepository.Save())
            {
                //return Ok(Mapper.Map<CustomerDTO>(customerEntity));
                return CreatedAtRoute("GetSingleCustomer", new {customerEntity.Id}, Mapper.Map<CustomerDTO>(customerEntity));
            }

            //return StatusCode(500, "Unable to Save Customer.");
            throw new Exception("Unable to Save Customer Data.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] UpdateCustomerDTO customerToUpdate)
        {
            var customerEntity = _customerRepository.GetSingle(id);

            if (customerEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(customerToUpdate, customerEntity);

            _customerRepository.Update(customerEntity);

            if (!_customerRepository.Save())
            {
                //return StatusCode(500);
                throw new Exception("An error while attempting to update Customer data. Please try again");
            }

            return Ok(Mapper.Map<UpdateCustomerDTO>(customerEntity));
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<UpdateCustomerDTO> customerPatchDocument)
        {
            if (customerPatchDocument == null)
            {
                return BadRequest();
            }

            var existingCustomer = _customerRepository.GetSingle(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerToPatch = Mapper.Map<UpdateCustomerDTO>(existingCustomer);
            customerPatchDocument.ApplyTo(customerToPatch);

            Mapper.Map(customerToPatch, existingCustomer);

            _customerRepository.Update(existingCustomer);

            if (!_customerRepository.Save())
            {
                //return StatusCode(500);
                throw new Exception("An error while attempting to update Customer data. Please try again.");
            }

            return Ok(Mapper.Map<CustomerDTO>(existingCustomer));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var customerToDelete = _customerRepository.GetSingle(id);

            if (customerToDelete == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(id);

            if (!_customerRepository.Save())
            {
                //return StatusCode(500);
                throw new Exception("An error while attempting to delete Customer data. Please try again.");
            }

            return NoContent();
        }

        
    }
}