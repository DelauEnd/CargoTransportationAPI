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
    public class CustomersController : ExtendedControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await repository.Customers.GetAllCustomersAsync(false);

            var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }

        [HttpGet("{Id}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetCustomerById(int Id)
        {
            var customer = await repository.Customers.GetCustomerByIdAsync(Id, false);
            if (customer == null)
                return NotFound(logInfo: true, nameof(customer));

            var customerDto = mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransport([FromBody]CustomerForCreation customer)
        {
            if (customer == null)
                return SendedIsNull(logError: true, nameof(customer));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(customer));

            var addableCustomer = mapper.Map<Customer>(customer);
            await CreateCustomerAsync(addableCustomer);

            var customerToReturn = mapper.Map<CustomerDto>(addableCustomer);
            return CustomerAdded(customerToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerById(int id)
        {
            var customer = await repository.Customers.GetCustomerByIdAsync(id, true);
            if (customer == null)
                return NotFound(logInfo: true, nameof(customer));

            await DeleteCustomerAsync(customer);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateCustomerById(int id, [FromBody]JsonPatchDocument<CustomerForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var customer = await repository.Customers.GetCustomerByIdAsync(id, true);
            if (customer == null)
                return NotFound(true, nameof(customer));

            PatchCustomer(patchDoc, customer);
            await repository.SaveAsync();

            return NoContent();
        }

        private void PatchCustomer(JsonPatchDocument<CustomerForUpdate> patchDoc, Customer customer)
        {
            var customerToPatch = mapper.Map<CustomerForUpdate>(customer);
            patchDoc.ApplyTo(customerToPatch, ModelState);

            TryToValidate(customerToPatch);

            mapper.Map(customerToPatch, customer);
        }

        private void TryToValidate(CustomerForUpdate orderToPatch)
        {
            TryValidateModel(orderToPatch);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");
        }

        private async Task CreateCustomerAsync(Customer customer)
        {
            repository.Customers.CreateCustomer(customer);
            await repository.SaveAsync();
        }

        private IActionResult CustomerAdded(CustomerDto customer)
        {
            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, customer);
        }

        private async Task DeleteCustomerAsync(Customer customer)
        {
            repository.Customers.DeleteCustomer(customer);
            await repository.SaveAsync();
        }
    }
}