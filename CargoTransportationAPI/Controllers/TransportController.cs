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

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Transport")]
    [ApiController]
    public class TransportController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public TransportController (IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            this.repository = repositoryManager;
            this.logger = loggerManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllTransport()
        {
            var transpor = repository.Transport.GetAllTransport(false);

            var transportDto = mapper.Map<IEnumerable<TransportDto>>(transpor);

            return Ok(transportDto);
        }

        [HttpGet("{Id}", Name = "GetTransportById")]
        public IActionResult GetTransportById(int Id)
        {
            var transport = repository.Transport.GetTransportById(Id, false);
            if (transport == null)
                return NotFound(logInfo: true, nameof(transport));
            
            var transportDto = mapper.Map<TransportDto>(transport);
            return Ok(transportDto);
        }

        [HttpPost]
        public IActionResult AddTransport([FromBody]TransportForCreation transport)
        {
            if (transport == null)
                return SendedIsNull(logError: true, nameof(transport));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(transport));

            var addableTransport = mapper.Map<Transport>(transport);
            CreateTransport(addableTransport);

            var transportToReturn = mapper.Map<TransportDto>(addableTransport);
            return TransportAdded(transportToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransportById(int id)
        {
            var transport = repository.Transport.GetTransportById(id, true);
            if (transport == null)
                return NotFound(logInfo: true, nameof(transport));

            DeleteTransport(transport);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateTransportById(int id, [FromBody]JsonPatchDocument<TransportForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var transport = repository.Transport.GetTransportById(id, true);
            if (transport == null)
                return NotFound(true, nameof(transport));


            PatchTransport(patchDoc, transport);
            repository.Save();

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

        private IActionResult NotFound(bool logInfo, string objName)
        {
            var message = $"The desired object({objName}) was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        private IActionResult UnprocessableEntity(bool logInfo, string objName)
        {
            var message = $"Object({objName}) has incorrect state";
            if (logInfo)
                logger.LogInfo(message);

            return UnprocessableEntity(ModelState);
        }

        private void DeleteTransport(Transport route)
        {
            repository.Transport.DeleteTransport(route);
            repository.Save();
        }

        private IActionResult SendedIsNull(bool logError, string objName)
        {
            var message = $"Sended {objName} is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private void CreateTransport(Transport transport)
        {
            repository.Transport.CreateTransport(transport);
            repository.Save();
        }

        private IActionResult TransportAdded(TransportDto transport)
        {
            return CreatedAtRoute("GetTransportById", new { id = transport.Id }, transport);
        }
    }
}