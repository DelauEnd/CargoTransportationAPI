﻿using System;
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
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public OrdersController(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            this.repository = repositoryManager;
            this.logger = loggerManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = repository.Orders.GetAllOrders(false);

            var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(ordersDto);
        }

        [HttpGet("{Id}", Name = "GetOrderById")]
        public IActionResult GetOrderById(int id)
        {
            var order = repository.Orders.GetOrderById(id, false);
            if (order == null)
                return NotFound(logInfo: true, nameof(order));

            var orderDto = mapper.Map<OrderWithCargoesDto>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderForCreation order)
        {
            if (order == null)
                return SendedIsNull(logError: true, nameof(order));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(order));

            var addableOrder = mapper.Map<Order>(order);
            CreateOrder(addableOrder);

            var orderToReturn = GetOrderToReturn(addableOrder);
            return OrderAdded(orderToReturn);
        }

        [HttpGet("{id}/Cargoes")]
        public IActionResult GetCargoesByRouteId([FromRoute]int id)
        {
            var cargoes = repository.Cargoes.GetCargoesByOrderId(id, false);
            if (cargoes == null)
                return NotFound(logInfo: true, nameof(cargoes));

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);
            return Ok(cargoesDto);
        }

        [HttpPost("{id}/Cargoes")]
        public IActionResult AddCargoes([FromBody] IEnumerable<CargoForCreation> cargoes, [FromRoute]int id)
        {
            if (cargoes == null)
                return SendedIsNull(logError: true, nameof(cargoes));

            if (!ModelState.IsValid)
                return UnprocessableEntity(true, nameof(cargoes));

            var addableCargoes = mapper.Map<IEnumerable<Cargo>>(cargoes);
            CreateCargoes(addableCargoes, id);

            var orderToReturn = GetCargoesToReturn(addableCargoes);
            return Ok(orderToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrderById(int id)
        {
            var order = repository.Orders.GetOrderById(id, true);
            if (order == null)
                return NotFound(logInfo: true, nameof(order));

            DeleteOrder(order);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateOrderById(int id, [FromBody]JsonPatchDocument<OrderForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return SendedIsNull(true, nameof(patchDoc));

            var order = repository.Orders.GetOrderById(id, true);
            if (order == null)
                return NotFound(true, nameof(order));

            PatchOrder(patchDoc, order);
            repository.Save();

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

        private IActionResult UnprocessableEntity(bool logInfo, string objName)
        {
            var message = $"Object({objName}) has incorrect state";
            if (logInfo)
                logger.LogInfo(message);

            return UnprocessableEntity(ModelState);
        }

        private IActionResult NotFound(bool logInfo, string objName)
        {
            var message = $"The desired object({objName}) was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        private void DeleteOrder(Order order)
        {
            repository.Orders.DeleteOrder(order);
            repository.Save();
        }

        private IActionResult SendedIsNull(bool logError, string objName)
        {
            var message = $"Sended {objName} is null";
            if (logError)
                logger.LogError(message);
            return BadRequest(message);
        }

        private void CreateOrder(Order order)
        {
            repository.Orders.CreateOrder(order);
            repository.Save();
        }

        private OrderWithCargoesDto GetOrderToReturn(Order addableOrder)
        {
            var order = IncludeData(addableOrder);
            return mapper.Map<OrderWithCargoesDto>(order);
        }

        private Order IncludeData(Order order)
        {
            var sender = repository.Customers.GetSenderByOrderId(order.Id, false);
            var destination = repository.Customers.GetDestinationByOrderId(order.Id, false);
            var cargoes = repository.Cargoes.GetCargoesByOrderId(order.Id, false);

            order.Cargoes = cargoes.ToList();
            order.Destination = destination;
            order.Sender = sender;

            return order;
        }

        private IActionResult OrderAdded(OrderWithCargoesDto order)
        {
            return CreatedAtRoute("GetOrderById", new { id = order.Id }, order);
        }

        private void CreateCargoes(IEnumerable<Cargo> cargoes, int id)
        {
            foreach (var cargo in cargoes)
                repository.Cargoes.CreateCargoForOrder(cargo, id);
            repository.Save();
        }

        private object GetCargoesToReturn(IEnumerable<Cargo> addableCargoes)
        {
            var cargoes = IncludeData(addableCargoes);
            return mapper.Map<IEnumerable<CargoDto>>(cargoes);
        }

        private IEnumerable<Cargo> IncludeData(IEnumerable<Cargo> cargoes)
        {
            foreach (var cargo in cargoes)
            {
                var cargoCategory = repository.CargoCategories.GetCategoryByCargoId(cargo.Id, false);
                cargo.Category = cargoCategory;
            }
            return cargoes;
        }
    }
}