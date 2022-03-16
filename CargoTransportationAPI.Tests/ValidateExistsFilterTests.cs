using CargoTransportationAPI.ActionFilters;
using CargoTransportationAPI.Controllers.v1;
using Interfaces;
using Entities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using Xunit;
using DTO.ResponseDTO;

namespace CargoTransportationAPI.Tests
{
    public class ValidateExistsFilterTests
    {
        [Fact]
        public async void ExistsCargoFilterIfNotExists()
        {
            //Arrange
            var id = 1;

            var mockLogger = new Mock<ILoggerManager>();
            var mockRepo = new Mock<IRepositoryManager>();
            mockRepo.Setup(repo => repo.Cargoes.GetCargoByIdAsync(id, false)).ReturnsAsync(null as Cargo);

            var arguments = new Dictionary<string, object>();
            arguments.Add("cargoId", id);
            var cargoController = new CargoesController(new Mock<IDataShaper<CargoDto>>().Object);

            var context = CreateContext(arguments, cargoController);
            var nextMock = new Mock<ActionExecutionDelegate>();

            var validator = new ValidateCargoExistsAttribute(mockLogger.Object, mockRepo.Object);

            //Act
            await validator.OnActionExecutionAsync(context, nextMock.Object);

            //Assert
            context.Result.Should().BeOfType<NotFoundResult>("because object with requested id not exists");
        }

        [Fact]
        public async void ExistsCargoFilterIfExists()
        {
            //Arrange
            var cargo = TestModels.TestCargo;

            var mockLogger = new Mock<ILoggerManager>();
            var mockRepo = new Mock<IRepositoryManager>();
            mockRepo.Setup(repo => repo.Cargoes.GetCargoByIdAsync(cargo.Id, false)).ReturnsAsync(cargo as Cargo);

            var arguments = new Dictionary<string, object>();
            arguments.Add("cargoId", cargo.Id);
            var cargoController = new CargoesController(new Mock<IDataShaper<CargoDto>>().Object);

            var context = CreateContext(arguments, cargoController);
            var nextMock = new Mock<ActionExecutionDelegate>();

            var validator = new ValidateCargoExistsAttribute(mockLogger.Object, mockRepo.Object);

            //Act
            await validator.OnActionExecutionAsync(context, nextMock.Object);

            //Assert
            context.HttpContext.Items.Should().ContainKey("cargo")
                .WhoseValue.Should().BeOfType(typeof(Cargo)).And.BeEquivalentTo(cargo);
        }

        public ActionExecutingContext CreateContext(Dictionary<string, object> arguments, object controller)
        {
            var actionContext = new ActionContext();
            actionContext.HttpContext = new DefaultHttpContext();
            actionContext.RouteData = new RouteData();
            actionContext.ActionDescriptor = new ActionDescriptor();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new Mock<IList<IFilterMetadata>>().Object, arguments, controller);

            return actionExecutingContext;
        }
    }
}