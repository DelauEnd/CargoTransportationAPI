using Entities.Models;
using Entities.RequestFeautures;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers.v2
{
    [ApiVersion("2")]
    [Route("api/{v:apiversion}/Cargoes")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CargoesController : ExtendedControllerBase
    {
        /// <summary>
        /// Get list of cargoes
        /// </summary>
        /// <returns>Returns cargoes list</returns>
        /// <response code="400">If incorrect date filter</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCargoes(CargoParameters parameters)
        {
            if (!parameters.IsValidDateFilter())
                return BadRequest("Date from cannot be later than date to");

            var cargoes = await repository.Cargoes.GetAllCargoesAsync(parameters, false);

            AddPaginationHeader(cargoes);

            return Ok(cargoes);
        }
    }
}