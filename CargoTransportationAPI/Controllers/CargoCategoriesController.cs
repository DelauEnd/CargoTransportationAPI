using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CargoTransportationAPI.ActionFilters;
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
        [HttpHead]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await repository.CargoCategories.GetAllCategoriesAsync(false);

            var categoriesDto = mapper.Map<IEnumerable<CargoCategoryDto>>(categories);

            return Ok(categoriesDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCategory([FromBody]CategoryForCreationDto category)
        {
            CargoCategory addableCategory = mapper.Map<CargoCategory>(category);
            await CreateCategoryAsync(addableCategory);

            var categoryToReturn = mapper.Map<CargoCategoryDto>(addableCategory);

            return Ok(categoryToReturn);
        }

        [HttpDelete("{categoryId}")]
        [ServiceFilter(typeof(ValidateCargoCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteCategoryById(int categoryId)
        {
            var category = HttpContext.Items["category"] as CargoCategory;

            await DeleteCategoryAsync(category);

            return NoContent();
        }

        [HttpPut("{categoryId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCargoCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateCargoCategoryById(int categoryId, CargoCategoryForUpdateDto category)
        {
            var categoryToUpdate = HttpContext.Items["category"] as CargoCategory;

            mapper.Map(category, categoryToUpdate);
            await repository.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCargoCategoryOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, PUT, DELETE, OPTIONS");
            return Ok();
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