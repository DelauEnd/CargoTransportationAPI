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
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await repository.CargoCategories.GetAllCategoriesAsync(false);

            var categoriesDto = mapper.Map<IEnumerable<CargoCategoryDto>>(categories);

            return Ok(categoriesDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]CategoryForCreation category)
        {
            if (category == null)
                return SendedIsNull(logError: true, nameof(category));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(category));

            CargoCategory addableCategory = mapper.Map<CargoCategory>(category);
            await CreateCategoryAsync(addableCategory);

            var categoryToReturn = mapper.Map<CargoCategoryDto>(addableCategory);

            return Ok(categoryToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            var category = await repository.CargoCategories.GetCategoryByIdAsync(id, true);
            if (category == null)
                return NotFound(logInfo: true, nameof(category));

            await DeleteCategoryAsync(category);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCargoCategoryById(int id, CargoCategoryForUpdate category)
        {
            if (category == null)
                return SendedIsNull(true, nameof(category));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(category));

            var categoryToUpdate = await repository.CargoCategories.GetCategoryByIdAsync(id, true);
            if (categoryToUpdate == null)
                return NotFound(true, nameof(categoryToUpdate));

            mapper.Map(category, categoryToUpdate);
            await repository.SaveAsync();

            return NoContent();
        }

        private async Task CreateCategoryAsync(CargoCategory category)
        {
            repository.CargoCategories.CreateCategory(category);
            await repository.SaveAsync();
        }

        private async Task DeleteCategoryAsync(CargoCategory category)
        {
            repository.CargoCategories.DeleteCategory(category);
            await repository.SaveAsync();
        }
    }
}