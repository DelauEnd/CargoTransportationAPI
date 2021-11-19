using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CargoTransportationAPI.ActionFilters;
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
        [HttpHead]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await repository.Customers.GetAllCustomersAsync(false);

            var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }

        [HttpGet("{customerId}", Name = "GetCustomerById")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateCustomerExistsAttribute))]
        public IActionResult GetCustomerById(int customerId)
        {
            var customer = HttpContext.Items["customer"] as Customer;

            var customerDto = mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddTransport([FromBody]CustomerForCreation customer)
        {
            var addableCustomer = mapper.Map<Customer>(customer);
            await CreateCustomerAsync(addableCustomer);

            var customerToReturn = mapper.Map<CustomerDto>(addableCustomer);
            return CustomerAdded(customerToReturn);
        }

        [HttpDelete("{customerId}")]
        [ServiceFilter(typeof(ValidateCustomerExistsAttribute))]
        public async Task<IActionResult> DeleteCustomerById(int customerId)
        {
            var customer = HttpContext.Items["customer"] as Customer;

            await DeleteCustomerAsync(customer);

            return NoContent();
        }

        [HttpPatch("{customerId}")]
        [ServiceFilter(typeof(ValidateCustomerExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCustomerById(int customerId, [FromBody]JsonPatchDocument<CustomerForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var customer = HttpContext.Items["customer"] as Customer;

            PatchCustomer(patchDoc, customer);
            await repository.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCustomerOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, PATCH, DELETE, OPTIONS");
            return Ok();
        }

        private void PatchCustomer(JsonPatchDocument<CustomerForUpdateDto> patchDoc, Customer customer)
        {
            var customerToPatch = mapper.Map<CustomerForUpdateDto>(customer);
            patchDoc.ApplyTo(customerToPatch, ModelState);

            TryToValidate(customerToPatch);

            mapper.Map(customerToPatch, customer);
        }

        private void TryToValidate(CustomerForUpdateDto orderToPatch)
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
            return CreatedAtRoute("GetCustomerById", new { customerId = customer.Id }, customer);
        }

        private async Task DeleteCustomerAsync(Customer customer)
        {
            repository.Customers.DeleteCustomer(customer);
            await repository.SaveAsync();
        }
    }
}