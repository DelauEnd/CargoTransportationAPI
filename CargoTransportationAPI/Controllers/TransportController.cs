using AutoMapper;
using CargoTransportationAPI.ActionFilters;
using CargoTransportationAPI.Extensions;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Transport"), Authorize]
    [ApiController]
    public class TransportController : ExtendedControllerBase
    {
        /// <summary>
        /// Get list of transport
        /// </summary>
        /// <returns>Returns transport list</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllTransport()
        {
            var transpor = await repository.Transport.GetAllTransportAsync(false);

            var transportDto = mapper.Map<IEnumerable<TransportDto>>(transpor);

            return Ok(transportDto);
        }

        /// <summary>
        /// Get transport by requested id
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>Returns transport by requested id</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested transport not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{transportId}", Name = "GetTransportById")]
        [HttpHead("{transportId}")]
        [ServiceFilter(typeof(ValidateTransportExistsAttribute))]
        public IActionResult GetTransportById(int transportId)
        {
            var transport = HttpContext.Items["transport"] as Transport;

            var transportDto = mapper.Map<TransportDto>(transport);
            return Ok(transportDto);
        }

        /// <summary>
        /// Create new transport
        /// </summary>
        /// <param name="transport"></param>
        /// <returns>Returns created transport</returns>
        /// <response code="400">If sended transport object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddTransportAsync([FromBody]TransportForCreationDto transport)
        {
            var addableTransport = mapper.Map<Transport>(transport);
            await CreateTransportAsync(addableTransport);   
            
            var transportToReturn = mapper.Map<TransportDto>(addableTransport);
            return TransportAdded(transportToReturn);
        }

        /// <summary>
        /// Delete transport by id
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested transport not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{transportId}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidateTransportExistsAttribute))]
        public async Task<IActionResult> DeleteTransportById(int transportId)
        {
            var transport = HttpContext.Items["transport"] as Transport;

            await DeleteTransportAsync(transport);

            return NoContent();
        }

        /// <summary>
        /// Update transport by id
        /// </summary>
        /// <param name="transportId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended pathDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested transport not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPatch("{transportId}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidateTransportExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateTransportById(int transportId, [FromBody]JsonPatchDocument<TransportForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var transport = HttpContext.Items["transport"] as Transport;

            PatchTransport(patchDoc, transport);
            await repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetTransportOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{transportId}")]
        public IActionResult GetTransportByIdOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, PATCH, DELETE, OPTIONS");
            return Ok();
        }

        private void PatchTransport(JsonPatchDocument<TransportForUpdateDto> patchDoc, Transport transport)
        {
            var orderToPatch = mapper.Map<TransportForUpdateDto>(transport);
            patchDoc.ApplyTo(orderToPatch, ModelState);

            TryToValidate(orderToPatch);

            mapper.Map(orderToPatch, transport);
        }

        private void TryToValidate(TransportForUpdateDto transportToPatch)
        {
            TryValidateModel(transportToPatch);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");
        }

        private async Task DeleteTransportAsync(Transport route)
        {
            repository.Transport.DeleteTransport(route);
            await repository.SaveAsync();
        }

        private async Task CreateTransportAsync(Transport transport)
        {
            repository.Transport.CreateTransport(transport);
            await repository.SaveAsync();
        }

        private IActionResult TransportAdded(TransportDto transport)
        {
            return CreatedAtRoute("GetTransportById", new { transportId = transport.Id }, transport);
        }
    }
}