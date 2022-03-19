using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Transport"), Authorize]
    [ApiController]
    public class TransportController : ControllerBase
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }

        /// <summary>
        /// Get list of transport
        /// </summary>
        /// <returns>Returns transport list</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllTransport()
        {
            var transport = await _transportService.GetAllTransport();
            return Ok(transport);
        }

        /// <summary>
        /// Get transport by requested id
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>Returns transport by requested id</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested transport not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{transportId}", Name = "GetTransportById")]
        [HttpHead("{transportId}")]
        public async Task<IActionResult> GetTransportById(int transportId)
        {
            var transport = await _transportService.GetTransportById(transportId);
            return Ok(transport);
        }

        /// <summary>
        /// Create new transport
        /// | Required role: Administrator
        /// </summary>
        /// <param name="transport"></param>
        /// <returns>Returns created transport</returns>
        /// <response code="400">If sended transport object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> AddTransport([FromBody] TransportForCreationDto transport)
        {
            await _transportService.AddTransport(transport);
            return Ok();
        }

        /// <summary>
        /// Delete transport by id
        /// | Required role: Administrator
        /// </summary>
        /// <param name="transportId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested transport not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{transportId}"), Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> DeleteTransportById(int transportId)
        {
            await _transportService.DeleteTransportById(transportId);
            return NoContent();
        }

        /// <summary>
        /// Update transport by id
        /// | Required role: Administrator
        /// </summary>
        /// <param name="transportId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended pathDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested transport not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPatch("{transportId}"), Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> PartiallyUpdateTransportById(int transportId, [FromBody] JsonPatchDocument<TransportForUpdateDto> patchDoc)
        {
            await _transportService.PatchTransportById(transportId, patchDoc);
            return NoContent();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetTransportOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{transportId}")]
        public IActionResult GetTransportByIdOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, PATCH, DELETE, OPTIONS");
            return Ok();
        }
    }
}