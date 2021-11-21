using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Entities.RequestFeautures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CargoTransportationAPI.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{v:apiversion}/Cargoes")]
    [ApiController]
    public class CargoesV2Controller : ExtendedControllerBase
    {
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCargoes([FromQuery]CargoParameters parameters)
        {
            if (!parameters.IsValidDateFilter())
                return BadRequest("date from cannot be later than date to");

            var cargoes = await repository.Cargoes.GetAllCargoesAsync(parameters, false);

            AddPaginationHeader(cargoes);

            return Ok(cargoes);
        }
    }
}