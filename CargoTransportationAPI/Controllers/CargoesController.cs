using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpDelete("{id}")]
        public IActionResult DeleteCargoById(int id)
        {
            var cargo = repository.Cargoes.GetCargoById(id, true);
            if (cargo == null)
                return NotFound(logInfo: true);

            DeleteCargo(cargo);

            return NoContent();
        }

        private IActionResult NotFound(bool logInfo)
        {
            var message = $"The desired object was not found";
            if (logInfo)
                logger.LogInfo(message);

            return NotFound();
        }

        private void DeleteCargo(Cargo cargo)
        {
            repository.Cargoes.DeleteCargo(cargo);
            repository.Save();
        }

        [HttpPut("UnmarkCargo")]
        public IActionResult UnmarkCargoFromRoute(int cargoId)
        {
            if (repository.Cargoes.GetCargoById(cargoId, false) == null)
                return NotFound(true);

            UnmarkTheCargoFromRoute(cargoId);

            return Ok();
        }

        private void UnmarkTheCargoFromRoute(int id)
        {
            repository.Cargoes.UnmarkTheCargoFromRoute(id);
            repository.Save();
        }
    }
}