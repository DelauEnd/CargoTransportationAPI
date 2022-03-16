using AutoMapper;
using CargoTransportationAPI.Controllers.v1;
using Interfaces;
using Entities.Models;
using Entities.RequestFeautures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using DTO.ResponseDTO;

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
            result.Should().BeOfType<BadRequestObjectResult>("because DateTime filter parameters invalid");
        }

        [Fact]
        public async void GetAllCargoesReturnsCorrectValue()
        {
            //Arrange
            var cargoesList = new List<Cargo>();
            cargoesList.Add(TestModels.TestCargo);
            var cargoes = cargoesList.ToPagedList<Cargo>(1, 10);

            var parameters = new CargoParameters();

            var mockRepo = new Mock<IRepositoryManager>();
            mockRepo.Setup(repo => repo.Cargoes.GetAllCargoesAsync(parameters, false)).ReturnsAsync(cargoes);

            CargoesController controller = SetupController();
            controller._repository = mockRepo.Object;

            //Act
            var result = await controller.GetAllCargoes(parameters);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        private static CargoesController SetupController()
        {
            var mockShaper = new Mock<IDataShaper<CargoDto>>();

            var controller = new CargoesController(mockShaper.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            var mockLogger = new Mock<ILoggerManager>();
            controller._logger = mockLogger.Object;
            var mockMapper = new Mock<IMapper>();
            controller._mapper = mockMapper.Object;

            return controller;
        }
    }
}
