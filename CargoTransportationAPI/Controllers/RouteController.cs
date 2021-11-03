using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Routes")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public RouteController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
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

        [HttpGet("{Id}", Name = "GetRouteWithCargoesById")]
        public IActionResult GetRouteWithCargoesById(int Id)
        {
            var route = repository.Routes.GetRouteById(Id, false);
            if (route == null)
                return NotFound(logInfo: true);
            var routeDto = mapper.Map<RouteWithCargoesDto>(route);

            return Ok(routeDto);
        }

        private IActionResult NotFound(bool logInfo)
        {
            var message = $"The desired object was not found";
            if (logInfo)
                logger.LogInfo(message);

            return NotFound();
        }

        [HttpPost]
        public IActionResult AddRoute([FromBody]RouteForCreation route)
        {
            if (route == null)
                return SendedIsNull(logError: true);

            Route addableRoute = RouteForCreationToRoute(route);
            CreateRoute(addableRoute);

            var routeToReturn = GetRouteToReturn(addableRoute);
            return RouteAdded(routeToReturn);
        }

        private IActionResult SendedIsNull(bool logError)
        {
            var message = $"Sended object is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private Route RouteForCreationToRoute(RouteForCreation routeForCreation)
        {
            var transport = repository.Transports.GetTransportByRegistrationNumber(routeForCreation.TransportRegistrationNumber,false);
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
            var transport = repository.Transports.GetTransportById(addableRoute.TransportId, false);
            addableRoute.Transport = transport;
            return mapper.Map<RouteDto>(addableRoute);
        }

        private IActionResult RouteAdded(RouteDto route)
        {
            return CreatedAtRoute("GetRouteWithCargoesById", new { id = route.Id }, route); ;
        }
    }
}