using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Categories")/*, Authorize*/]
    [ApiController]
    public class CargoCategoriesController : ControllerBase
    {
        private readonly ICargoCategoryService _cargoCategoryService;
        public CargoCategoriesController(ICargoCategoryService cargoCategoryService)
        {
            _cargoCategoryService = cargoCategoryService;
        }

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
            var categories = await _cargoCategoryService.GetAllCategories();
            return Ok(categories);
        }

        /// <summary>
        /// Create category
        /// | Required role: Administrator
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Returns requested category</returns>
        /// <response code="400">If sended category object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> AddCategory([FromBody] CategoryForCreationDto category)
        {
            await _cargoCategoryService.AddCategory(category);
            return Ok(category);
        }

        /// <summary>
        /// Delete category by id
        /// | Required role: Administrator
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested category not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{categoryId}"), Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> DeleteCategoryById(int categoryId)
        {
            await _cargoCategoryService.DeleteCategoryById(categoryId);
            return NoContent();
        }

        /// <summary>
        /// Update category by id
        /// | Required role: Administrator
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested category not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPut("{categoryId}"), Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> UpdateCargoCategoryById(int categoryId, CargoCategoryForUpdateDto category)
        {
            await _cargoCategoryService.UpdateCargoCategoryById(categoryId, category);
            return Ok();
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
    }
}