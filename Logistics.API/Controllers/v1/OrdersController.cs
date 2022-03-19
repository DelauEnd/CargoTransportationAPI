using Logistics.Models.Enums;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.RequestDTO.UpdateDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logistics.API.Controllers.v1
{
    [Route("api/Orders"), Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get list of orders
        /// </summary>
        /// <returns>Returns orders list</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        /// <summary>
        /// Get order by requested id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Returns order by requested id</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{orderId}", Name = "GetOrderById")]
        [HttpHead("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return Ok(order);
        }

        /// <summary>
        /// Create new order
        /// | Required role: Manager
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Returns created order</returns>
        /// <response code="400">If sended order object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> AddOrder([FromBody] OrderForCreationDto orderToAdd)
        {
            await _orderService.AddOrder(orderToAdd);
            return Ok();
        }

        /// <summary>
        /// Get cargoes by requested order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Returns cargoes by requested order id</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{orderId}/Cargoes")]
        [HttpHead("{orderId}/Cargoes")]
        public async Task<IActionResult> GetCargoesByOrderId([FromRoute] int orderId)
        {
            var cargoes = await _orderService.GetCargoesByOrderId(orderId);
            return Ok(cargoes);
        }

        /// <summary>
        /// Add cargoes to order
        /// | Required role: Manager
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cargoes"></param>
        /// <returns>Returns updated order</returns>
        /// <response code="400">If sended cargoes object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost("{orderId}/Cargoes"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> AddCargoes([FromBody] IEnumerable<CargoForCreationDto> cargoes, [FromRoute] int orderId)
        {
            await _orderService.AddCargoesToOrder(cargoes, orderId);
            return Ok();
        }

        /// <summary>
        /// Delete order by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{orderId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> DeleteOrderById(int orderId)
        {
            await _orderService.DeleteOrderById(orderId);
            return NoContent();
        }

        /// <summary>
        /// Update order by id
        /// | Required role: Manager
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended pathDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPatch("{orderId}"), Authorize(Roles = nameof(UserRole.Manager))]
        public async Task<IActionResult> PartiallyUpdateOrderById(int orderId, [FromBody] JsonPatchDocument<OrderForUpdateDto> patchDoc)
        {
            await _orderService.PatchOrderById(orderId, patchDoc);
            return NoContent();
        }

        /// <summary>
        /// Get allowed requests
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions]
        public IActionResult GetOrderOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for id
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{orderId}")]
        public IActionResult GetOrderByIdOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, PATCH, DELETE, OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Get allowed requests for cargoes
        /// </summary>
        /// <returns>Returns allowed requests</returns>
        [HttpOptions("{orderId}/Cargoes")]
        public IActionResult GetOrderByIdWithCargoesOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, POST, OPTIONS");
            return Ok();
        }
    }
}