using AutoMapper;
using CargoTransportationAPI.Controllers;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeautures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CargoTransportationAPI.Tests
{
    public class CargoesControllerTests
    {
        [Fact]
        public async void BadRequestOnGetAllCargoesIfIncorrectDateFilter()
        {
            //Arrange
            CargoesController controller = SetupController();

            var mockRepo = new Mock<IRepositoryManager>();
            controller._repository = mockRepo.Object;

            var parameters = new CargoParameters
            {
                ArrivalDateFrom = DateTime.Now,
                ArrivalDateTo = DateTime.Now.AddDays(-5)
            };

            //Act
            var result = await controller.GetAllCargoes(parameters);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void GetAllCargoesReturnsCorrectValue()
        {
            //Arrange
            CargoesController controller = SetupController();

            var cargoesList = new List<Cargo>();
            cargoesList.Add(TestModels.TestCargo());
            var cargoes = cargoesList.ToPagedList<Cargo>(1, 10);

            var parameters = new CargoParameters();

            var mockRepo = new Mock<IRepositoryManager>();
            mockRepo.Setup(repo => repo.Cargoes.GetAllCargoesAsync(parameters, false)).ReturnsAsync(cargoes);

            controller._repository = mockRepo.Object;

            //Act
            var result = await controller.GetAllCargoes(parameters);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var objResult = result as OkObjectResult;
            Assert.Equal(cargoes, objResult.Value);
        }

        private static CargoesController SetupController()
        {
            var mockShaper = new Mock<IDataShaper<CargoDto>>();

            var controller = new CargoesController(mockShaper.Object);

            var mockLogger = new Mock<ILoggerManager>();
            controller._logger = mockLogger.Object;
            var mockMapper = new Mock<IMapper>();
            controller._mapper = mockMapper.Object;

            return controller;
        }
    }
}
