using AutoMapper;
using CargoTransportationAPI.Extensions;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
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
            var transpor = repository.Transports.GetAllTransport(false);

            var transportDto = mapper.Map<IEnumerable<TransportDto>>(transpor);

            return Ok(transportDto);
        }

        [HttpGet("{Id}", Name = "GetTransportById")]
        public IActionResult GetTransportById(int Id)
        {
            var transport = repository.Transports.GetTransportById(Id, false);
            if (transport == null)
                return NotFound(logInfo: true);
            
            var transportDto = mapper.Map<TransportDto>(transport);
            return Ok(transportDto);
        }

        private IActionResult NotFound(bool logInfo)
        {
            var message = $"The desired object was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddTransport([FromBody]TransportForCreation transport)
        {
            if (transport == null)
                return SendedIsNull(logError: true);

            var addableTransport = mapper.Map<Transport>(transport);
            CreateTransport(addableTransport);

            var transportToReturn = mapper.Map<TransportDto>(addableTransport);
            return TransportAdded(transportToReturn);
        }

        private IActionResult SendedIsNull(bool logError)
        {
            var message = $"Sended object is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private void CreateTransport(Transport transport)
        {
            repository.Transports.CreateTransport(transport);
            repository.Save();
        }

        private IActionResult TransportAdded(TransportDto transport)
        {
            return CreatedAtRoute("GetTransportById", new { id = transport.Id }, transport);
        }
    }
}