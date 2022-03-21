﻿using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Categories"), Authorize]
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