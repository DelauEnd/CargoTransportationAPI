using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Routes"), Authorize]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        /// <summary>
        /// Get list of routes
        /// </summary>
        /// <returns>Returns routes list</returns>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _routeService.GetAllRoutes();
            return Ok(routes);
        }

        /// <summary>
        /// Get route by requested id
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>Returns route by requested id</returns>
        [HttpGet("{routeId}", Name = "GetRouteById")]
        [HttpHead("{routeId}")]
        public async Task<IActionResult> GetRouteById(int routeId)
        {
            var route = await _routeService.GetRouteById(routeId);
            return Ok(route);
        }

        /// <summary>
        /// Create new route
        /// | Required role: Manager
        /// </summary>
        /// <param name="route"></param>
        [HttpPost, Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> AddRoute([FromBody] RouteForCreationDto route)
        {
            await _routeService.AddRoute(route);
            return Ok();
        }

        /// <summary>
        /// Delete route by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="routeId"></param>
        [HttpDelete("{routeId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> DeleteRouteById(int routeId)
        {
            await _routeService.DeleteRouteById(routeId);
            return NoContent();
        }

        /// <summary>
        /// Update route by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="route"></param>
        [HttpPut("{routeId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> UpdateRouteById(int routeId, RouteForUpdateDto route)
        {
            await _routeService.UpdateRouteById(routeId, route);
            return NoContent();
        }

        /// <summary>
        /// Get cargoes by requested route id
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>Returns cargoes by requested order id</returns>
        [HttpGet("{routeId}/Cargoes")]
        [HttpHead("{routeId}/Cargoes")]
        public async Task<IActionResult> GetCargoesByRouteId(int routeId)
        {
            var cargoes = await _routeService.GetCargoesByRouteId(routeId);
            return Ok(cargoes);
        }

        /// <summary>
        /// Mark cargo by requested id to route by requested id
        /// | Required role: Manager
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="routeId"></param>
        [HttpPost("{routeId}/Cargoes"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> AssignCargoesToRoute([FromBody] List<int> ids, int routeId)
        {
            await _routeService.AssignCargoesToRoute(ids, routeId);
            return Ok();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetRouteOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{routeId}")]
        public IActionResult GetRouteByIdOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, PUT, DELETE, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for cargoes
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{routeId}/Cargoes")]
        public IActionResult GetRouteByIdWithCargoesOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }
    }
}