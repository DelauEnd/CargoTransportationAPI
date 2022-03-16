using AutoMapper;
using DTO.RequestDTO.UpdateDTO;
using DTO.ResponseDTO;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeautures;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logistics.Controllers.v1
{
    [Route("api/Cargoes"), Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CargoesController : ControllerBase
    {
        public readonly IDataShaper<CargoDto> cargoDataShaper;
        public readonly IRepositoryManager repository;
        public readonly IMapper mapper;

        public CargoesController(IDataShaper<CargoDto> cargoDataShaper, IRepositoryManager repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.cargoDataShaper = cargoDataShaper;
        }

        /// <summary>
        /// Get list of cargo categories
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Returns cargo categories list</returns>
        /// <response code="400">If incorrect date filter</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCargoes([FromQuery] CargoParameters parameters)
        {
            if (!parameters.IsValidDateFilter())
                return BadRequest("Date from cannot be later than date to");

            var cargoes = await repository.Cargoes.GetAllCargoesAsync(parameters, false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoDataShaper.ShapeData(cargoesDto, parameters.Fields));
        }

        /// <summary>
        /// Get list of cargo categories
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Returns cargo categories list</returns>
        /// <response code="400">If incorrect date filter</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        [Route("Unassigned")]
        public async Task<IActionResult> GetUnassignedCargoes([FromQuery] CargoParameters parameters)
        {
            if (!parameters.IsValidDateFilter())
                return BadRequest("Date from cannot be later than date to");

            var cargoes = await repository.Cargoes.GetUnassignedCargoesAsync(parameters, false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoDataShaper.ShapeData(cargoesDto, parameters.Fields));
        }

        /// <summary>
        /// Get cargo by id
        /// </summary>
        /// <param name="cargoId"></param>
        /// <param name="parameters"></param>
        /// <returns>Returns requested cargo</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested cargo not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{cargoId}")]
        [HttpHead("{cargoId}")]
        public IActionResult GetCargoById(int cargoId, [FromQuery] CargoParameters parameters)
        {
            var cargo = repository.Cargoes.GetCargoByIdAsync(cargoId, false);

            var cargoDto = mapper.Map<CargoDto>(cargo);

            return Ok(cargoDataShaper.ShapeData(cargoDto, parameters.Fields));
        }

        /// <summary>
        /// Delete cargo by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="cargoId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested cargo not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{cargoId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> DeleteCargoById(int cargoId)
        {
            var cargo = await repository.Cargoes.GetCargoByIdAsync(cargoId, false);

            await DeleteCargoAsync(cargo);

            return NoContent();
        }

        /// <summary>
        /// Update cargo by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="cargoId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended pathDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested cargo not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPatch("{cargoId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> PartiallyUpdateCargoById(int cargoId, [FromBody] JsonPatchDocument<CargoForUpdateDto> patchDoc)
        {
            var cargo = await repository.Cargoes.GetCargoByIdAsync(cargoId, false);

            var cargoToPatch = mapper.Map<CargoForUpdateDto>(cargo);
            patchDoc.ApplyTo(cargoToPatch, ModelState);

            TryValidateModel(cargoToPatch);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");

            mapper.Map(cargoToPatch, cargo);

            await repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetCargoOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{cargoId}")]
        public IActionResult GetCargoByIdOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, PATCH, DELETE, OPTIONS");
            return Ok();
        }

        private void PatchCargo(JsonPatchDocument<CargoForUpdateDto> patchDoc, Cargo cargo)
        {
            var cargoToPatch = mapper.Map<CargoForUpdateDto>(cargo);
            patchDoc.ApplyTo(cargoToPatch, ModelState);

            TryValidateModel(cargoToPatch);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");

            mapper.Map(cargoToPatch, cargo);
        }


        private async Task DeleteCargoAsync(Cargo cargo)
        {
            repository.Cargoes.DeleteCargo(cargo);
            await repository.SaveAsync();
        }
    }
}