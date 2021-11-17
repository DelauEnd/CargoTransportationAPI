using AutoMapper;
using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Routes")]
    [ApiController]
    public class RoutesController : ExtendedControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRoutes()
        {
            var route = await repository.Routes.GetAllRoutesAsync(false);

            var routeDto = mapper.Map<IEnumerable<RouteDto>>(route);

            return Ok(routeDto);
        }

        [HttpGet("{routeId}", Name = "GetRouteById")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public IActionResult GetRouteById(int routeId)
        {
            var route = HttpContext.Items["route"] as Route;

            var routeDto = mapper.Map<RouteDto>(route);

            return Ok(routeDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddRoute([FromBody]RouteForCreationDto route)
        {
            Route addableRoute = RouteForCreationToRoute(route);
            await CreateRouteAsync(addableRoute);

            var routeToReturn = await GetRouteToReturnAsync(addableRoute);
            return RouteAdded(routeToReturn);
        }

        [HttpDelete("{routeId}")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public async Task<IActionResult> DeleteRouteById(int routeId)
        {
            var route = HttpContext.Items["route"] as Route;

            await DeleteRouteAsync(route);

            return NoContent();
        }

        [HttpPut("{routeId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public async Task<IActionResult> UpdateRouteById(int routeId, RouteForUpdateDto route)
        {
            var routeToUpdate = HttpContext.Items["route"] as Route;

            UpdateRoute(route, routeToUpdate);
            await repository.SaveAsync();

            return NoContent();
        }

        [HttpGet("{routeId}/Cargoes")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        public async Task<IActionResult> GetCargoesByRouteId(int routeId)
        {
            var route = HttpContext.Items["route"] as Route;

            var cargoes = await repository.Cargoes.GetCargoesByRouteIdAsync(route.Id, false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoesDto);
        }

        [HttpPost("{routeId}/Cargoes/MarkCargo")]
        [ServiceFilter(typeof(ValidateRouteExistsAttribute))]
        [ServiceFilter(typeof(ValidateCargoExistsAttribute))]
        public async Task<IActionResult> MarkCargoToRouteAsync(int routeId, int cargoId)
        {
            var route = HttpContext.Items["route"] as Route;

            var cargo = HttpContext.Items["cargo"] as Cargo;

            await MarkTheCargoToRouteAsync(route.Id, cargo.Id);

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

        private async Task MarkTheCargoToRouteAsync(int routeId, int cargoId)
        {
            await repository.Cargoes.MarkTheCargoToRouteAsync(cargoId, routeId);
            await repository.SaveAsync();
        }
    }
}