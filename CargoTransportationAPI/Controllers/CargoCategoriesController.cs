using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Cargoes/Categories")]
    [ApiController]
    public class CargoCategoriesController : ExtendedControllerBase
    {
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
                return SendedIsNull(logError: true, nameof(category));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(category));

            CargoCategory addableCategory = mapper.Map<CargoCategory>(category);
            CreateCategory(addableCategory);

            var categoryToReturn = mapper.Map<CargoCategoryDto>(addableCategory);

            return Ok(categoryToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoryById(int id)
        {
            var category = repository.CargoCategories.GetCategoryById(id, true);
            if (category == null)
                return NotFound(logInfo: true, nameof(category));

            DeleteCategory(category);

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCargoCategoryById(int id, CargoCategoryForUpdate category)
        {
            if (category == null)
                return SendedIsNull(true, nameof(category));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(category));

            var categoryToUpdate = repository.CargoCategories.GetCategoryById(id, true);
            if (categoryToUpdate == null)
                return NotFound(true, nameof(categoryToUpdate));

            mapper.Map(category, categoryToUpdate);
            repository.Save();

            return NoContent();
        }

        private void CreateCategory(CargoCategory category)
        {
            repository.CargoCategories.CreateCategory(category);
            repository.Save();
        }

        private void DeleteCategory(CargoCategory category)
        {
            repository.CargoCategories.DeleteCategory(category);
            repository.Save();
        }
    }
}