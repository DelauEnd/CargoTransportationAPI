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

        [HttpGet("{Id}")]
        public IActionResult GetTransportById(int Id)
        {
            var transport = repository.Transports.GetTransportById(Id, false);
            if (transport == null)
            {
                logger.LogInfo($"There is no Transport with Id {Id}");
                return NotFound();
            }
            else
            {
                var transportDto = mapper.Map<TransportDto>(transport);
                return Ok(transportDto);
            }
        }


    }
}