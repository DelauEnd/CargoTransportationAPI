using AutoMapper;
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
        public async Task<IActionResult> GetAllTransport()
        {
            var transpor = await repository.Transport.GetAllTransportAsync(false);

            var transportDto = mapper.Map<IEnumerable<TransportDto>>(transpor);

            return Ok(transportDto);
        }

        [HttpGet("{Id}", Name = "GetTransportById")]
        public async Task<IActionResult> GetTransportById(int Id)
        {
            var transport = await repository.Transport.GetTransportByIdAsync(Id, false);
            if (transport == null)
                return NotFound(logInfo: true, nameof(transport));
            
            var transportDto = mapper.Map<TransportDto>(transport);
            return Ok(transportDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransportAsync([FromBody]TransportForCreation transport)
        {
            if (transport == null)
                return SendedIsNull(logError: true, nameof(transport));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(transport));

            var addableTransport = mapper.Map<Transport>(transport);
            await CreateTransportAsync(addableTransport);

            var transportToReturn = mapper.Map<TransportDto>(addableTransport);
            return TransportAdded(transportToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportById(int id)
        {
            var transport = await repository.Transport.GetTransportByIdAsync(id, true);
            if (transport == null)
                return NotFound(logInfo: true, nameof(transport));

            await DeleteTransportAsync(transport);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateTransportById(int id, [FromBody]JsonPatchDocument<TransportForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var transport = await repository.Transport.GetTransportByIdAsync(id, true);
            if (transport == null)
                return NotFound(true, nameof(transport));


            PatchTransport(patchDoc, transport);
            await repository.SaveAsync();

            return NoContent();
        }

        private void PatchTransport(JsonPatchDocument<TransportForUpdate> patchDoc, Transport transport)
        {
            var orderToPatch = mapper.Map<TransportForUpdate>(transport);
            patchDoc.ApplyTo(orderToPatch, ModelState);

            TryToValidate(orderToPatch);

            mapper.Map(orderToPatch, transport);
        }

        private void TryToValidate(TransportForUpdate transportToPatch)
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
            return CreatedAtRoute("GetTransportById", new { id = transport.Id }, transport);
        }
    }
}