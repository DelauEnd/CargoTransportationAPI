using CargoTransportationAPI.ActionFilters;
using CargoTransportationAPI.Controllers;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using Xunit;

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
            Assert.IsType<NotFoundResult>(context.Result);
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