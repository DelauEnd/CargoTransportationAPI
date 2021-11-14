using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Cargoes/Categories")]
    [ApiController]
    public class CargoCategoriesController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public CargoCategoriesController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            this.repository = repositoryManager;
            this.logger = loggerManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = repository.CargoCategories.GetAllCategories(false);

            var categoriesDto = mapper.Map<IEnumerable<CargoCategoryDto>>(categories);

            return Ok(categoriesDto);
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody]CategoryForCreation category)
        {
            if (category == null)
                return SendedIsNull(logError: true);

            CargoCategory addableCategory = mapper.Map<CargoCategory>(category);
            CreateCategory(addableCategory);

            var categoryToReturn = mapper.Map<CargoCategoryDto>(addableCategory);

            return Ok(categoryToReturn);
        }

        private IActionResult SendedIsNull(bool logError)
        {
            var message = $"Sended object is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private void CreateCategory(CargoCategory category)
        {
            repository.CargoCategories.CreateCategory(category);
            repository.Save();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoryById(int id)
        {
            var category = repository.CargoCategories.GetCategoryById(id, true);
            if (category == null)
                return NotFound(logInfo: true);

            DeleteCategory(category);

            return NoContent();
        }

        private IActionResult NotFound(bool logInfo)
        {
            var message = $"The desired object was not found";
            if (logInfo)
                logger.LogInfo(message);

            return NotFound();
        }

        private void DeleteCategory(CargoCategory category)
        {
            repository.CargoCategories.DeleteCategory(category);
            repository.Save();
        }
    }
}