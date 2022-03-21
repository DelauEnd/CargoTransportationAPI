using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Cargoes"), Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CargoesController : ControllerBase
    {
        private readonly ICargoService _cargoService;

        public CargoesController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        /// <summary>
        /// Get list of cargo categories
        /// </summary>
        /// <returns>Returns cargo categories list</returns>
        /// <response code="400">If incorrect date filter</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCargoes()
        {
            return Ok(await _cargoService.GetAllCargoes());
        }

        /// <summary>
        /// Get list of cargo categories
        /// </summary>
        /// <returns>Returns cargo categories list</returns>
        /// <response code="400">If incorrect date filter</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        [Route("Unassigned")]
        public async Task<IActionResult> GetUnassignedCargoes()
        {
            var cargoes = await _cargoService.GetUnassignedCargoes();
            return Ok(cargoes);
        }

        /// <summary>
        /// Get cargo by id
        /// </summary>
        /// <param name="cargoId"></param>
        /// <returns>Returns requested cargo</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested cargo not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{cargoId}")]
        [HttpHead("{cargoId}")]
        public async Task<IActionResult> GetCargoById(int cargoId)
        {
            var cargo = await _cargoService.GetCargoById(cargoId);
            return Ok(cargo);
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
            await _cargoService.DeleteCargoById(cargoId);
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
            await _cargoService.PatchCargoById(cargoId, patchDoc);
            return Ok();
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
    }
}