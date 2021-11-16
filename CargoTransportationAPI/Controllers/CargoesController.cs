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
    public class CargoesController : ExtendedControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCargoes()
        {
            var cargoes = await repository.Cargoes.GetAllCargoesAsync(false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoesDto);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCargoById(int id)
        {
            var cargo = await repository.Cargoes.GetCargoByIdAsync(id, false);
            if (cargo == null)
                return NotFound(logInfo: true, nameof(cargo));

            var cargoDto = mapper.Map<CargoDto>(cargo);
            return Ok(cargoDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargoById(int id)
        {
            var cargo = await repository.Cargoes.GetCargoByIdAsync(id, true);
            if (cargo == null)
                return NotFound(logInfo: true, nameof(cargo));

            await DeleteCargoAsync(cargo);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateCargoById(int id, [FromBody]JsonPatchDocument<CargoForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var cargo = await repository.Cargoes.GetCargoByIdAsync(id, true);
            if (cargo == null)
                return NotFound(true, nameof(cargo));

            PatchCargo(patchDoc, cargo);
            await repository.SaveAsync();

            return NoContent();
        }

        private void PatchCargo(JsonPatchDocument<CargoForUpdate> patchDoc, Cargo cargo)
        {
            var cargoToPatch = mapper.Map<CargoForUpdate>(cargo);
            patchDoc.ApplyTo(cargoToPatch, ModelState);

            TryToValidate(cargoToPatch);

            mapper.Map(cargoToPatch, cargo);
        }

        private void TryToValidate(CargoForUpdate cargoToPatch)
        {
            TryValidateModel(cargoToPatch);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");
        }

        private async Task DeleteCargoAsync(Cargo cargo)
        {
            repository.Cargoes.DeleteCargo(cargo);
            await repository.SaveAsync();
        }
    }
}