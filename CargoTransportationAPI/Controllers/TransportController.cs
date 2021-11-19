using AutoMapper;
using CargoTransportationAPI.ActionFilters;
using CargoTransportationAPI.Extensions;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Transport")]
    [ApiController]
    public class TransportController : ExtendedControllerBase
    {
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllTransport()
        {
            var transpor = await repository.Transport.GetAllTransportAsync(false);

            var transportDto = mapper.Map<IEnumerable<TransportDto>>(transpor);

            return Ok(transportDto);
        }

        [HttpGet("{transportId}", Name = "GetTransportById")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateTransportExistsAttribute))]
        public IActionResult GetTransportById(int transportId)
        {
            var transport = HttpContext.Items["transport"] as Transport;

            var transportDto = mapper.Map<TransportDto>(transport);
            return Ok(transportDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddTransportAsync([FromBody]TransportForCreationDto transport)
        {
            var addableTransport = mapper.Map<Transport>(transport);
            await CreateTransportAsync(addableTransport);

            var transportToReturn = mapper.Map<TransportDto>(addableTransport);
            return TransportAdded(transportToReturn);
        }

        [HttpDelete("{transportId}")]
        [ServiceFilter(typeof(ValidateTransportExistsAttribute))]
        public async Task<IActionResult> DeleteTransportById(int transportId)
        {
            var transport = HttpContext.Items["transport"] as Transport;

            await DeleteTransportAsync(transport);

            return NoContent();
        }

        [HttpPatch("{transportId}")]
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

        [HttpOptions]
        public IActionResult GetTransportOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, PATCH, DELETE, OPTIONS");
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