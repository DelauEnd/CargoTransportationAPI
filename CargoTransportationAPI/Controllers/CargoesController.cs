using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using CargoTransportationAPI.ActionFilters;
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

        [HttpGet("{cargoId}")]
        [ServiceFilter(typeof(ValidateCargoExistsAttribute))]
        public IActionResult GetCargoById(int cargoId)
        {
            var cargo = HttpContext.Items["cargo"] as Cargo;

            var cargoDto = mapper.Map<CargoDto>(cargo);
            return Ok(cargoDto);
        }

        [HttpDelete("{cargoId}")]
        [ServiceFilter(typeof(ValidateCargoExistsAttribute))]
        public async Task<IActionResult> DeleteCargoById(int cargoId)
        {
            var cargo = HttpContext.Items["cargo"] as Cargo;

            await DeleteCargoAsync(cargo);

            return NoContent();
        }

        [HttpPatch("{cargoId}")]
        [ServiceFilter(typeof(ValidateCargoExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCargoById(int cargoId, [FromBody]JsonPatchDocument<CargoForUpdateDto> patchDoc)
        {
            var cargo = HttpContext.Items["cargo"] as Cargo;

            PatchCargo(patchDoc, cargo);
            await repository.SaveAsync();

            return NoContent();
        }

        private void PatchCargo(JsonPatchDocument<CargoForUpdateDto> patchDoc, Cargo cargo)
        {
            var cargoToPatch = mapper.Map<CargoForUpdateDto>(cargo);
            patchDoc.ApplyTo(cargoToPatch, ModelState);

            TryToValidate(cargoToPatch);

            mapper.Map(cargoToPatch, cargo);
        }

        private void TryToValidate(CargoForUpdateDto cargoToPatch)
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