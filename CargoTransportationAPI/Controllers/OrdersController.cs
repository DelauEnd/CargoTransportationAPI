using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
using Entities.Models;
using Entities.RequestFeautures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Orders"), Authorize]
    [ApiController]
    public class OrdersController : ExtendedControllerBase
    {
        private readonly IDataShaper<CargoDto> cargoDataShaper;

        public OrdersController(IDataShaper<CargoDto> cargoDataShaper)
        {
            this.cargoDataShaper = cargoDataShaper;
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
            var orders = await repository.Orders.GetAllOrdersAsync(false);

            var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(ordersDto);
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
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public IActionResult GetOrderById(int orderId)
        {
            var order = HttpContext.Items["order"] as Order;

            var orderDto = mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Returns created order</returns>
        /// <response code="400">If sended order object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost, Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddOrder([FromBody] OrderForCreationDto order)
        {
            var addableOrder = mapper.Map<Order>(order);
            await CreateOrderAsync(addableOrder);

            var orderToReturn = await GetOrderToReturnAsync(addableOrder);
            return OrderAdded(orderToReturn);
        }

        /// <summary>
        /// Get cargoes by requested order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="parameters"></param>
        /// <returns>Returns cargoes by requested order id</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="500">Unhandled exception</response>
        [HttpGet("{orderId}/Cargoes")]
        [HttpHead("{orderId}/Cargoes")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> GetCargoesByOrderId([FromRoute]int orderId, [FromQuery]CargoParameters parameters)
        {
            var order = HttpContext.Items["order"] as Order;

            var cargoes = await repository.Cargoes.GetCargoesByOrderIdAsync(order.Id, parameters, false);

            AddPaginationHeader(cargoes);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);
            return Ok(cargoDataShaper.ShapeData(cargoesDto, parameters.Fields));
        }

        /// <summary>
        /// Add cargoes to order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cargoes"></param>
        /// <returns>Returns updated order</returns>
        /// <response code="400">If sended cargoes object is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost("{orderId}/Cargoes"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> AddCargoesAsync([FromBody] IEnumerable<CargoForCreationDto> cargoes, [FromRoute]int orderId)
        {
            var order = HttpContext.Items["order"] as Order;

            var addableCargoes = mapper.Map<IEnumerable<Cargo>>(cargoes);
            await CreateCargoesAsync(addableCargoes, order.Id);

            var orderToReturn = await GetCargoesToReturnAsync(addableCargoes);
            return Ok(orderToReturn);
        }

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Returns if deleted successfully</returns>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpDelete("{orderId}"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> DeleteOrderById(int orderId)
        {
            var order = HttpContext.Items["order"] as Order;

            await DeleteOrderAsync(order);

            return NoContent();
        }

        /// <summary>
        /// Update order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns if updated successfully</returns>
        /// <response code="400">If sended pathDoc is null</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If requested order not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPatch("{orderId}"), Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateOrderById(int orderId, [FromBody]JsonPatchDocument<OrderForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var order = HttpContext.Items["order"] as Order;

            PatchOrder(patchDoc, order);
            await repository.SaveAsync();

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

        private void PatchOrder(JsonPatchDocument<OrderForUpdateDto> patchDoc, Order order)
        {
            var orderToPatch = mapper.Map<OrderForUpdateDto>(order);
            patchDoc.ApplyTo(orderToPatch, ModelState);

            TryToValidate(orderToPatch);

            mapper.Map(orderToPatch, order);
        }

        private void TryToValidate(OrderForUpdateDto orderToPatch)
        {
            TryValidateModel(orderToPatch);
            if (!ModelState.IsValid)
                throw new Exception("InvalidModelState");
        }

        private async Task DeleteOrderAsync(Order order)
        {
            repository.Orders.DeleteOrder(order);
            await repository.SaveAsync();
        }

        private async Task CreateOrderAsync(Order order)
        {
            repository.Orders.CreateOrder(order);
            await repository.SaveAsync();
        }

        private async Task<OrderDto> GetOrderToReturnAsync(Order addableOrder)
        {
            var order = await IncludeDataAsync(addableOrder);
            return mapper.Map<OrderDto>(order);
        }

        private async Task<Order> IncludeDataAsync(Order order)
        {
            var sender = await repository.Customers.GetSenderByOrderIdAsync(order.Id, false);
            var destination = await repository.Customers.GetDestinationByOrderIdAsync(order.Id, false);

            order.Destination = destination;
            order.Sender = sender;

            return order;
        }

        private IActionResult OrderAdded(OrderDto order)
        {
            return CreatedAtRoute("GetOrderById", new { orderId = order.Id }, order);
        }

        private async Task CreateCargoesAsync(IEnumerable<Cargo> cargoes, int orderId)
        {
            foreach (var cargo in cargoes)
                repository.Cargoes.CreateCargoForOrder(cargo, orderId);
            await repository.SaveAsync();
        }

        private async Task<object> GetCargoesToReturnAsync(IEnumerable<Cargo> addableCargoes)
        {
            var cargoes = await IncludeDataAsync(addableCargoes);
            return mapper.Map<IEnumerable<CargoDto>>(cargoes);
        }

        private async Task<IEnumerable<Cargo>> IncludeDataAsync(IEnumerable<Cargo> cargoes)
        {
            foreach (var cargo in cargoes)
            {
                var cargoCategory = await repository.CargoCategories.GetCategoryByCargoIdAsync(cargo.Id, false);
                cargo.Category = cargoCategory;
            }
            return cargoes;
        }
    }
}