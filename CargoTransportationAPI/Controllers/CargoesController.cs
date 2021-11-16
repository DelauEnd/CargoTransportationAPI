using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    [Route("api/Cargoes")]
    [ApiController]
    public class CargoesController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public CargoesController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            this.repository = repositoryManager;
            this.logger = loggerManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCargoes()
        {
            var cargoes = repository.Cargoes.GetAllCargoes(false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoesDto);
        }

        [HttpGet("{Id}")]
        public IActionResult GetCargoById(int id)
        {
            var cargo = repository.Cargoes.GetCargoById(id, false);
            if (cargo == null)
                return NotFound(logInfo: true, nameof(cargo));

            var cargoDto = mapper.Map<CargoDto>(cargo);
            return Ok(cargoDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCargoById(int id)
        {
            var cargo = repository.Cargoes.GetCargoById(id, true);
            if (cargo == null)
                return NotFound(logInfo: true, nameof(cargo));

            DeleteCargo(cargo);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCargoById(int id, [FromBody]JsonPatchDocument<CargoForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var cargo = repository.Cargoes.GetCargoById(id, true);
            if (cargo == null)
                return NotFound(true, nameof(cargo));

            PatchCargo(patchDoc, cargo);
            repository.Save();

            return NoContent();
        }

        private void PatchCargo(JsonPatchDocument<CargoForUpdate> patchDoc, Cargo cargo)
        {
            var cargoToPatch = mapper.Map<CargoForUpdate>(cargo);
            patchDoc.ApplyTo(cargoToPatch);
            mapper.Map(cargoToPatch, cargo);
        }

        private IActionResult SendedIsNull(bool logError, string objName)
        {
            var message = $"Sended {objName} is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private IActionResult NotFound(bool logInfo, string objName)
        {
            var message = $"The desired object({objName}) was not found";
            if (logInfo)
                logger.LogInfo(message);

            return NotFound();
        }

        private void DeleteCargo(Cargo cargo)
        {
            repository.Cargoes.DeleteCargo(cargo);
            repository.Save();
        }
    }
}