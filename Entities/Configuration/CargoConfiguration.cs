using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Configuration
{
    public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            AddInitialData(builder);
        }

        private void AddInitialData(EntityTypeBuilder<Cargo> builder)
        {
            builder.HasData
            (
                new Cargo
                {
                    Id = 1,
                    Title = "Initial Cargo",
                    CategoryId = 1,
                    DepartureDate = DateTime.Now,
                    ArrivalDate = DateTime.Now.AddDays(10),
                    RouteId = 1,
                    OrderId = 1,
                    Weight = 200,
                }
            );
            builder.OwnsOne(Cargo => Cargo.Dimensions).HasData(
                new //По возможности избавиться от анонимного типа
                {
                    CargoId = 1,
                    Height = 50d,
                    Width = 50d,
                    Length = 50d
                }
            );
        }
    }
}
