using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{Id}")]
        public IActionResult GetRouteWithCargoesById(int Id)
        {
            var route = repository.Routes.GetRouteById(Id, false);

            var routeDto = mapper.Map<RouteWithCargoesDto>(route);

            return Ok(routeDto);
        }
    }
}