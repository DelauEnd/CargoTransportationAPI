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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ExtendedControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await repository.Orders.GetAllOrdersAsync(false);

            var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(ordersDto);
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public IActionResult GetOrderById(int orderId)
        {
            var order = HttpContext.Items["order"] as Order;

            var orderDto = mapper.Map<OrderWithCargoesDto>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddOrder([FromBody] OrderForCreationDto order)
        {
            var addableOrder = mapper.Map<Order>(order);
            await CreateOrderAsync(addableOrder);

            var orderToReturn = await GetOrderToReturnAsync(addableOrder);
            return OrderAdded(orderToReturn);
        }

        [HttpGet("{orderId}/Cargoes")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> GetCargoesByOrderId([FromRoute]int orderId)
        {
            var order = HttpContext.Items["order"] as Order;

            var cargoes = await repository.Cargoes.GetCargoesByOrderIdAsync(order.Id, false);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);
            return Ok(cargoesDto);
        }

        [HttpPost("{orderId}/Cargoes")]
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

        [HttpDelete("{orderId}")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> DeleteOrderById(int orderId)
        {
            var order = HttpContext.Items["order"] as Order;

            await DeleteOrderAsync(order);

            return NoContent();
        }

        [HttpPatch("{orderId}")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateOrderById(int orderId, [FromBody]JsonPatchDocument<OrderForUpdateDto> patchDoc)
        {
            var order = HttpContext.Items["order"] as Order;

            PatchOrder(patchDoc, order);
            await repository.SaveAsync();

            return NoContent();
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

        private async Task<OrderWithCargoesDto> GetOrderToReturnAsync(Order addableOrder)
        {
            var order = await IncludeDataAsync(addableOrder);
            return mapper.Map<OrderWithCargoesDto>(order);
        }

        private async Task<Order> IncludeDataAsync(Order order)
        {
            var sender = await repository.Customers.GetSenderByOrderIdAsync(order.Id, false);
            var destination = await repository.Customers.GetDestinationByOrderIdAsync(order.Id, false);
            var cargoes = await repository.Cargoes.GetCargoesByOrderIdAsync(order.Id, false);

            order.Cargoes = cargoes.ToList();
            order.Destination = destination;
            order.Sender = sender;

            return order;
        }

        private IActionResult OrderAdded(OrderWithCargoesDto order)
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