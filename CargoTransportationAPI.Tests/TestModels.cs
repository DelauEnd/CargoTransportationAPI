using Entities.Models;
using System;

namespace CargoTransportationAPI.Tests
{
    public class TestModels
    {
        public static Cargo TestCargo
            = new Cargo
            {
                ArrivalDate = DateTime.Now,
                DepartureDate = DateTime.Now.AddDays(-5),
                CategoryId = 1,
                Dimensions = new Dimensions
                {
                    Height = 1,
                    Width = 1,
                    Length = 1
                },
                Id = 1,
                OrderId = 1,
                RouteId = 1,
                Title = "title",
                Weight = 1
            };
    }
}
