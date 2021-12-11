using CargoTransportationAPI.ActionFilters;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Cargoes/Categories"), Authorize]
    [ApiController]
    public class CargoCategoriesController : ExtendedControllerBase
    {
        /// <summary>
        /// Get list of categories
        /// </summary>
        /// <returns>Returns cargoes list</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await repository.CargoCategories.GetAllCategoriesAsync(false);

            var categoriesDto = mapper.Map<IEnumerable<CargoCategoryDto>>(categories);

            return Ok(categoriesDto);
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Returns requested category</returns>
        /// <response code="400">If sended category object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCategory([FromBody]CategoryForCreationDto category)
        {
            CargoCategory addableCategory = mapper.Map<CargoCategory>(category);
            await CreateCategoryAsync(addableCategory);

            var categoryToReturn = mapper.Map<CargoCategoryDto>(addableCategory);

            return Ok(categoryToReturn);
        }

        /// <summary>
        /// Delete category by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested category not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{categoryId}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidateCargoCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteCategoryById(int categoryId)
        {
            var category = HttpContext.Items["category"] as CargoCategory;

            await DeleteCategoryAsync(category);

            return NoContent();
        }

        /// <summary>
        /// Update category by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested category not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPut("{categoryId}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCargoCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateCargoCategoryById(int categoryId, CargoCategoryForUpdateDto category)
        {
            var categoryToUpdate = HttpContext.Items["category"] as CargoCategory;

            mapper.Map(category, categoryToUpdate);
            await repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetCargoCategoryOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{categoryId}")]
        public IActionResult GetCargoCategoryByIdOptions()
        {
            Response.Headers.Add("Allow", "PUT, DELETE, OPTIONS");
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