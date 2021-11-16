using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedControllerBase : ControllerBase
    {
        private IRepositoryManager _repository;
        protected IRepositoryManager repository => _repository ?? (_repository = HttpContext.RequestServices.GetService<IRepositoryManager>());

        private ILoggerManager _logger;
        protected ILoggerManager logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILoggerManager>());

        private IMapper _mapper;
        protected IMapper mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());

        protected IActionResult UnprocessableEntity(bool logInfo, string objName)
        {
            var message = $"Object({objName}) has incorrect state";
            if (logInfo)
                logger.LogInfo(message);
            return UnprocessableEntity(ModelState);
        }

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
    }
}