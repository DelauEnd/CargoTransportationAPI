using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Routes")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public RoutesController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            this.repository = repositoryManager;
            this.logger = loggerManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllRoutes()
        {
            var route = repository.Routes.GetAllRoutes(false);

            var routeDto = mapper.Map<IEnumerable<RouteDto>>(route);

            return Ok(routeDto);
        }

        [HttpGet("{id}", Name = "GetRouteById")]
        public IActionResult GetRouteById(int id)
        {
            var route = repository.Routes.GetRouteById(id, false);
            if (route == null)
                return NotFound(logInfo: true, nameof(route));
            var routeDto = mapper.Map<RouteDto>(route);

            return Ok(routeDto);
        }

        [HttpPost]
        public IActionResult AddRoute([FromBody]RouteForCreation route)
        {
            if (route == null)
                return SendedIsNull(logError: true, nameof(route));

            Route addableRoute = RouteForCreationToRoute(route);
            CreateRoute(addableRoute);

            var routeToReturn = GetRouteToReturn(addableRoute);
            return RouteAdded(routeToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRouteById(int id)
        {
            var route = repository.Routes.GetRouteById(id, true);
            if (route == null)
                return NotFound(logInfo: true, nameof(route));

            DeleteRoute(route);

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRouteById(int id, RouteForUpdate route)
        {
            if (route == null)
                return SendedIsNull(true, nameof(route));

            var routeToUpdate = repository.Routes.GetRouteById(id, true);
            if (routeToUpdate == null)
                return NotFound(true, nameof(routeToUpdate));

            UpdateRoute(route, routeToUpdate);
            repository.Save();

            return NoContent();
        }

        private void UpdateRoute(RouteForUpdate route, Route routeToUpdate)
        {
            var transport = repository.Transport.GetTransportByRegistrationNumber(route.TransportRegistrationNumber, false);         
            routeToUpdate.TransportId = transport.Id;
        }

        [HttpGet("{id}/Cargoes")]
        public IActionResult GetCargoesByRouteId(int id)
        {
            var cargoes = repository.Cargoes.GetCargoesByRouteId(id, false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);

            return Ok(cargoesDto);
        }

        [HttpPost("{routeId}/Cargoes/MarkCargo")]
        public IActionResult MarkCargoToRoute(int routeId, int cargoId)
        {
            if (repository.Routes.GetRouteById(routeId, false) == null)
                return NotFound(false);

            if (repository.Cargoes.GetCargoById(cargoId, false) == null)
                return BadRequest();

            MarkTheCargoToRoute(routeId, cargoId);

            return Ok();
        }

        private IActionResult NotFound(bool logInfo, string objName)
        {
            var message = $"The desired object({objName}) was not found";
            if (logInfo)
                logger.LogInfo(message);

            return NotFound();
        }

        private IActionResult SendedIsNull(bool logError, string objName)
        {
            var message = $"Sended {objName} is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private Route RouteForCreationToRoute(RouteForCreation routeForCreation)
        {
            var transport = repository.Transport.GetTransportByRegistrationNumber(routeForCreation.TransportRegistrationNumber, false);
            Route route = new Route
            {
                TransportId = transport.Id,
            };
            return route;
        }

        private void CreateRoute(Route route)
        {
            repository.Routes.CreateRoute(route);
            repository.Save();
        }

        private RouteDto GetRouteToReturn(Route addableRoute)
        {
            var transport = repository.Transport.GetTransportById(addableRoute.TransportId, false);
            addableRoute.Transport = transport;
            return mapper.Map<RouteDto>(addableRoute);
        }

        private IActionResult RouteAdded(RouteDto route)
        {
            return CreatedAtRoute("GetRouteById", new { id = route.Id }, route); ;
        }

        private void DeleteRoute(Route route)
        {
            repository.Routes.DeleteRoute(route);
            repository.Save();
        }

        private void MarkTheCargoToRoute(int routeId, int cargoId)
        {
            repository.Cargoes.MarkTheCargoToRoute(cargoId, routeId);
            repository.Save();
        }
    }
}