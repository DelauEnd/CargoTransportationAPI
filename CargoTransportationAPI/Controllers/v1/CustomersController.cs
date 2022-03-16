using AutoMapper;
using DTO.RequestDTO.CreateDTO;
using DTO.RequestDTO.UpdateDTO;
using DTO.ResponseDTO;
using Entities.Enums;
using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logistics.Controllers.v1
{
    [Route("api/Customers"), Authorize]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public readonly IRepositoryManager repository;
        public readonly IMapper mapper;

        public CustomersController(IRepositoryManager repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// Get list of customers
        /// </summary>
        /// <returns>Returns customers list</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await repository.Customers.GetAllCustomersAsync(false);

            var customersDto = mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Returns requested customer</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested customer not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{customerId}", Name = "GetCustomerById")]
        [HttpHead("{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var customer = await repository.Customers.GetCustomerByIdAsync(customerId, false);

            var customerDto = mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        /// <summary>
        /// Create new customer by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Returns created customer</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerForCreationDto customer)
        {
            var addableCustomer = mapper.Map<Customer>(customer);
            await CreateCustomerAsync(addableCustomer);

            var customerToReturn = mapper.Map<CustomerDto>(addableCustomer);
            return CreatedAtRoute("GetCustomerById", new { customerId = customerToReturn.Id }, customer);
        }

        /// <summary>
        /// Delete customer by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested customer not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{customerId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> DeleteCustomerById(int customerId)
        {
            var customer = await repository.Customers.GetCustomerByIdAsync(customerId, false);

            await DeleteCustomerAsync(customer);

            return NoContent();
        }

        /// <summary>
        /// Update customer by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended pathDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested customer not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPatch("{customerId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> PartiallyUpdateCustomerById(int customerId, [FromBody] JsonPatchDocument<CustomerForUpdateDto> patchDoc)
        {
            var customer = await repository.Customers.GetCustomerByIdAsync(customerId, false);

            var customerToPatch = mapper.Map<CustomerForUpdateDto>(customer);
            patchDoc.ApplyTo(customerToPatch, ModelState);

            TryValidateModel(customer);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");

            mapper.Map(customerToPatch, customer);

            await repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetCustomerOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{customerId}")]
        public IActionResult GetCustomerByIdOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, DELETE, PATCH, OPTIONS");
            return Ok();
        }

        private async Task CreateCustomerAsync(Customer customer)
        {
            repository.Customers.CreateCustomer(customer);
            await repository.SaveAsync();
        }

        private async Task DeleteCustomerAsync(Customer customer)
        {
            repository.Customers.DeleteCustomer(customer);
            await repository.SaveAsync();
        }
    }
}