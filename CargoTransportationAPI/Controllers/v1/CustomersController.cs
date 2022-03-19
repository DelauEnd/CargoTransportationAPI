﻿using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Customers"), Authorize]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
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
            var customers = await _customerService.GetAllCustomers();
            return Ok(customers);
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
            var customer = await _customerService.GetCustomerById(customerId);
            return Ok(customer);
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
            await _customerService.AddCustomer(customer);
            return Ok(customer);
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
            await _customerService.DeleteCustomerById(customerId);
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
            await _customerService.PatchCustomerById(customerId, patchDoc);
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
    }
}