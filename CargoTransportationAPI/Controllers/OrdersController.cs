using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpDelete("{id}")]
        public IActionResult DeleteOrderById(int id)
        {
            var order = repository.Orders.GetOrderById(id, true);
            if (order == null)
                return NotFound(logInfo: true);

            DeleteOrder(order);

            return NoContent();
        }

        private void DeleteOrder(Order order)
        {
            repository.Orders.DeleteOrder(order);
            repository.Save();
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
                return NotFound(logInfo: true);

            var orderDto = mapper.Map<OrderWithCargoesDto>(order);
            return Ok(orderDto);
        }

        private IActionResult NotFound(bool logInfo)
        {
            var message = $"The desired object was not found";
            if (logInfo)
                logger.LogInfo(message);
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderForCreation order)
        {
            if (order == null)
                return SendedIsNull(logError: true);

            var addableOrder = mapper.Map<Order>(order);
            CreateOrder(addableOrder);

            var orderToReturn = GetOrderToReturn(addableOrder);
            return OrderAdded(orderToReturn);
        }

        private IActionResult SendedIsNull(bool logError)
        {
            var message = $"Sended object is null";
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

        [HttpGet("{id}/Cargoes")]
        public IActionResult GetCargoesByRouteId([FromRoute]int id)
        {
            var cargoes = repository.Cargoes.GetCargoesByOrderId(id, false);
            if (cargoes == null)
                return NotFound(logInfo: true);

            var cargoesDto = mapper.Map<IEnumerable<CargoDto>>(cargoes);
            return Ok(cargoesDto);
        }

        [HttpPost("{id}/Cargoes")]
        public IActionResult AddCargoes([FromBody] IEnumerable<CargoForCreation> cargoes, [FromRoute]int id)
        {
            if (cargoes == null)
                return SendedIsNull(logError: true);

            var addableCargoes = mapper.Map<IEnumerable<Cargo>>(cargoes);
            CreateCargoes(addableCargoes, id);

            var orderToReturn = GetCargoesToReturn(addableCargoes);
            return Ok(orderToReturn);
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