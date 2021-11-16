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

        [HttpGet("{Id}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await repository.Orders.GetOrderByIdAsync(id, false);
            if (order == null)
                return NotFound(logInfo: true, nameof(order));

            var orderDto = mapper.Map<OrderWithCargoesDto>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderForCreation order)
        {
            if (order == null)
                return SendedIsNull(logError: true, nameof(order));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(order));

            var addableOrder = mapper.Map<Order>(order);
            await CreateOrderAsync(addableOrder);

            var orderToReturn = await GetOrderToReturnAsync(addableOrder);
            return OrderAdded(orderToReturn);
        }

        [HttpGet("{id}/Cargoes")]
        public async Task<IActionResult> GetCargoesByRouteId([FromRoute]int id)
        {
            var cargoes = await repository.Cargoes.GetCargoesByOrderIdAsync(id, false);
            if (cargoes == null)
                return NotFound(logInfo: true, nameof(cargoes));

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);
            return Ok(cargoesDto);
        }

        [HttpPost("{id}/Cargoes")]
        public async Task<IActionResult> AddCargoesAsync([FromBody] IEnumerable<CargoForCreation> cargoes, [FromRoute]int id)
        {
            if (cargoes == null)
                return SendedIsNull(logError: true, nameof(cargoes));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(cargoes));

            var addableCargoes = mapper.Map<IEnumerable<Cargo>>(cargoes);
            await CreateCargoesAsync(addableCargoes, id);

            var orderToReturn = await GetCargoesToReturnAsync(addableCargoes);
            return Ok(orderToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            var order = await repository.Orders.GetOrderByIdAsync(id, true);
            if (order == null)
                return NotFound(logInfo: true, nameof(order));

            await DeleteOrderAsync(order);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateOrderById(int id, [FromBody]JsonPatchDocument<OrderForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var order = await repository.Orders.GetOrderByIdAsync(id, true);
            if (order == null)
                return NotFound(true, nameof(order));

            PatchOrder(patchDoc, order);
            await repository.SaveAsync();

            return NoContent();
        }

        private void PatchOrder(JsonPatchDocument<OrderForUpdate> patchDoc, Order order)
        {
            var orderToPatch = mapper.Map<OrderForUpdate>(order);
            patchDoc.ApplyTo(orderToPatch, ModelState);          

            TryToValidate(orderToPatch);

            mapper.Map(orderToPatch, order);
        }

        private void TryToValidate(OrderForUpdate orderToPatch)
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
            return CreatedAtRoute("GetOrderById", new { id = order.Id }, order);
        }

        private async Task CreateCargoesAsync(IEnumerable<Cargo> cargoes, int id)
        {
            foreach (var cargo in cargoes)
                repository.Cargoes.CreateCargoForOrder(cargo, id);
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