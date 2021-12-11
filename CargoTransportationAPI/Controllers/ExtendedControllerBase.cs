using AutoMapper;
using Contracts;
using Entities.RequestFeautures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CargoTransportationAPI.Controllers
{
    public class ExtendedControllerBase : ControllerBase
    {
        private IRepositoryManager _repository;
        protected IRepositoryManager repository => _repository ?? (_repository = HttpContext.RequestServices.GetService<IRepositoryManager>());

        private ILoggerManager _logger;
        protected ILoggerManager logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILoggerManager>());

        private IMapper _mapper;
        protected IMapper mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());

        protected IActionResult NotFound(bool logInfo, string objName)
        {
            var message = $"The desired object({objName}) was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        protected IActionResult SendedIsNull(bool logError, string objName)
        {
            var message = $"Sended {objName} is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        protected void AddPaginationHeader<T>(PagedList<T> elem)
        {
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(elem.MetaData));
        }
    }
}