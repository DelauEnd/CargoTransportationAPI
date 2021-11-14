using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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
                return NotFound(logInfo: true);

            var customerDto = mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        private IActionResult NotFound(bool logInfo)
        {
            var message = $"The desired object was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddTransport([FromBody]CustomerForCreation customer)
        {
            if (customer == null)
                return SendedIsNull(logError: true);

            var addableCustomer = mapper.Map<Customer>(customer);
            CreateCustomer(addableCustomer);

            var customerToReturn = mapper.Map<CustomerDto>(addableCustomer);
            return CustomerAdded(customerToReturn);
        }

        private IActionResult SendedIsNull(bool logError)
        {
            var message = $"Sended object is null";
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

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerById(int id)
        {
            var customer = repository.Customers.GetCustomerById(id, true);
            if (customer == null)
                return NotFound(logInfo: true);

            DeleteCustomer(customer);

            return NoContent();
        }

        private void DeleteCustomer(Customer customer)
        {
            repository.Customers.DeleteCustomer(customer);
            repository.Save();
        }
    }
}