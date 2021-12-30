using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Entities.RequestFeautures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Routes"), Authorize]
    [ApiController]
    public class RoutesController : ExtendedControllerBase
    {
        private readonly IDataShaper<CargoDto> cargoDataShaper;

        public RoutesController(IDataShaper<CargoDto> cargoDataShaper)
        {
            this.cargoDataShaper = cargoDataShaper;
        }

        /// <summary>
        /// Get list of routes
        /// </summary>
        /// <returns>Returns routes list</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllRoutes()
        {
            var route = await repository.Routes.GetAllRoutesAsync(false);

            var routeDto = mapper.Map<IEnumerable<RouteDto>>(route);

            return Ok(routeDto);
        }

        /// <summary>
        /// Get route by requested id
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>Returns route by requested id</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested route not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{routeId}", Name = "GetRouteById")]
        [HttpHead("{routeId}")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public IActionResult GetRouteById(int routeId)
        {
            var route = HttpContext.Items["route"] as Route;

            var routeDto = mapper.Map<RouteDto>(route);

            return Ok(routeDto);
        }

        /// <summary>
        /// Create new route
        /// </summary>
        /// <param name="route"></param>
        /// <returns>Returns created route</returns>
        /// <response code="400">If sended route object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddRoute([FromBody]RouteForCreationDto route)
        {
            Route addableRoute = RouteForCreationToRoute(route);
            await CreateRouteAsync(addableRoute);

            var routeToReturn = await GetRouteToReturnAsync(addableRoute);
            return RouteAdded(routeToReturn);
        }

        /// <summary>
        /// Delete route by id
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested route not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{routeId}"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public async Task<IActionResult> DeleteRouteById(int routeId)
        {
            var route = HttpContext.Items["route"] as Route;

            await DeleteRouteAsync(route);

            return NoContent();
        }

        /// <summary>
        /// Update route by id
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="route"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended route object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested route not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPut("{routeId}"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public async Task<IActionResult> UpdateRouteById(int routeId, RouteForUpdateDto route)
        {
            var routeToUpdate = HttpContext.Items["route"] as Route;

            UpdateRoute(route, routeToUpdate);
            await repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Get cargoes by requested route id
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="parameters"></param>
        /// <returns>Returns cargoes by requested order id</returns>
        /// <response code="400">If sended patchDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested route not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{routeId}/Cargoes")]
        [HttpHead("{routeId}/Cargoes")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public async Task<IActionResult> GetCargoesByRouteId(int routeId, [FromQuery]CargoParameters parameters)
        {
            var route = HttpContext.Items["route"] as Route;

            var cargoes = await repository.Cargoes.GetCargoesByRouteIdAsync(route.Id, parameters, false);

            AddPaginationHeader(cargoes);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoDataShaper.ShapeData(cargoesDto, parameters.Fields));
        }

        /// <summary>
        /// Mark cargo by requested id to route by requested id
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="cargoId"></param>
        /// <returns>Returns if marked successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested route or cargo not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost("{routeId}/Cargoes"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        [ServiceFilter(typeof(ValidateCargoExistsAttribute))]
        public IActionResult MarkCargoToRoute(int routeId, int cargoId)
        {
            var route = HttpContext.Items["route"] as Route;

            var cargo = HttpContext.Items["cargo"] as Cargo;

            repository.Cargoes.MarkTheCargoToRoute(cargo.Id, route.Id);

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

        private void UpdateRoute(RouteForUpdateDto route, Route routeToUpdate)
        {
            var transport = GetTransportByRegNumberAsync(route.TransportRegistrationNumber);

            routeToUpdate.TransportId = transport.Id;
        }

        private Route RouteForCreationToRoute(RouteForCreationDto routeForCreation)
        {
            var transport = GetTransportByRegNumberAsync(routeForCreation.TransportRegistrationNumber);

            Route route = new Route
            {
                TransportId = transport.Id,
            };
            return route;
        }

        private async Task<Transport> GetTransportByRegNumberAsync(string number)
        {
            var transport = await repository.Transport.GetTransportByRegistrationNumberAsync(number, false);
            if (transport == null)
                throw new Exception($"Transport with registration number {number} not exist");
            return transport;
        }

        private async Task CreateRouteAsync(Route route)
        {
            repository.Routes.CreateRoute(route);
            await repository.SaveAsync();
        }

        private async Task<RouteDto> GetRouteToReturnAsync(Route addableRoute)
        {
            var transport = await repository.Transport.GetTransportByIdAsync(addableRoute.TransportId, false);
            addableRoute.Transport = transport;
            return mapper.Map<RouteDto>(addableRoute);
        }

        private IActionResult RouteAdded(RouteDto route)
        {
            return CreatedAtRoute("GetRouteById", new { routeId = route.Id }, route); ;
        }

        private async Task DeleteRouteAsync(Route route)
        {
            repository.Routes.DeleteRoute(route);
            await repository.SaveAsync();
        }
    }
}