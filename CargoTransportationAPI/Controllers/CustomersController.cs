using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public CustomersController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            this.repository = repositoryManager;
            this.logger = loggerManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = repository.Customers.GetAllCustomers(false);

            var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }

        [HttpGet("{Id}", Name = "GetCustomerById")]
        public IActionResult GetCustomerById(int Id)
        {
            var customer = repository.Customers.GetCustomerById(Id, false);
            if (customer == null)
                return NotFound(logInfo: true, nameof(customer));

            var customerDto = mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public IActionResult AddTransport([FromBody]CustomerForCreation customer)
        {
            if (customer == null)
                return SendedIsNull(logError: true, nameof(customer));

            var addableCustomer = mapper.Map<Customer>(customer);
            CreateCustomer(addableCustomer);

            var customerToReturn = mapper.Map<CustomerDto>(addableCustomer);
            return CustomerAdded(customerToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerById(int id)
        {
            var customer = repository.Customers.GetCustomerById(id, true);
            if (customer == null)
                return NotFound(logInfo: true, nameof(customer));

            DeleteCustomer(customer);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCustomerById(int id, [FromBody]JsonPatchDocument<CustomerForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var customer = repository.Customers.GetCustomerById(id, true);
            if (customer == null)
                return NotFound(true, nameof(customer));

            PatchCustomer(patchDoc, customer);
            repository.Save();

            return NoContent();
        }

        private void PatchCustomer(JsonPatchDocument<CustomerForUpdate> patchDoc, Customer customer)
        {
            var customerToPatch = mapper.Map<CustomerForUpdate>(customer);
            patchDoc.ApplyTo(customerToPatch);
            mapper.Map(customerToPatch, customer);
        }

        private IActionResult NotFound(bool logInfo, string objName)
        {
            var message = $"The desired object({objName}) was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        private IActionResult SendedIsNull(bool logError, string objName)
        {
            var message = $"Sended {objName} is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private void CreateCustomer(Customer customer)
        {
            repository.Customers.CreateCustomer(customer);
            repository.Save();
        }

        private IActionResult CustomerAdded(CustomerDto customer)
        {
            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, customer);
        }

        private void DeleteCustomer(Customer customer)
        {
            repository.Customers.DeleteCustomer(customer);
            repository.Save();
        }
    }
}