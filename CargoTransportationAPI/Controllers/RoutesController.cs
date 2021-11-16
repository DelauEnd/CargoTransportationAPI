using AutoMapper;
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

        [HttpGet("{id}", Name = "GetRouteById")]
        public async Task<IActionResult> GetRouteById(int id)
        {
            var route = await repository.Routes.GetRouteByIdAsync(id, false);
            if (route == null)
                return NotFound(logInfo: true, nameof(route));
            var routeDto = mapper.Map<RouteDto>(route);

            return Ok(routeDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoute([FromBody]RouteForCreation route)
        {
            if (route == null)
                return SendedIsNull(logError: true, nameof(route));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(route));

            Route addableRoute = RouteForCreationToRoute(route);
            await CreateRouteAsync(addableRoute);

            var routeToReturn = await GetRouteToReturnAsync(addableRoute);
            return RouteAdded(routeToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRouteById(int id)
        {
            var route = await repository.Routes.GetRouteByIdAsync(id, true);
            if (route == null)
                return NotFound(logInfo: true, nameof(route));

            await DeleteRouteAsync(route);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRouteById(int id, RouteForUpdate route)
        {
            if (route == null)
                return SendedIsNull(true, nameof(route));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(route));

            var routeToUpdate = await repository.Routes.GetRouteByIdAsync(id, true);
            if (routeToUpdate == null)
                return NotFound(true, nameof(routeToUpdate));


            UpdateRoute(route, routeToUpdate);
            await repository.SaveAsync();

            return NoContent();
        }

        [HttpGet("{id}/Cargoes")]
        public async Task<IActionResult> GetCargoesByRouteId(int id)
        {
            var cargoes = await repository.Cargoes.GetCargoesByRouteIdAsync(id, false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoesDto);
        }

        [HttpPost("{routeId}/Cargoes/MarkCargo")]
        public async Task<IActionResult> MarkCargoToRouteAsync(int routeId, int cargoId)
        {
            if (await repository.Routes.GetRouteByIdAsync(routeId, false) == null)
                return NotFound(false);

            if (await repository.Cargoes.GetCargoByIdAsync(cargoId, false) == null)
                return BadRequest();

            await MarkTheCargoToRouteAsync(routeId, cargoId);

            return Ok();
        }
    
        private void UpdateRoute(RouteForUpdate route, Route routeToUpdate)
        {
            var transport = GetTransportByRegNumberAsync(route.TransportRegistrationNumber);

            routeToUpdate.TransportId = transport.Id;
        }

        private Route RouteForCreationToRoute(RouteForCreation routeForCreation)
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
            return CreatedAtRoute("GetRouteById", new { id = route.Id }, route); ;
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