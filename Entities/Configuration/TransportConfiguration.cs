using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    class TransportConfiguration : IEntityTypeConfiguration<Transport>
    {
        public void Configure(EntityTypeBuilder<Transport> builder)
        {
            AddInitialData(builder);
        }

        private void AddInitialData(EntityTypeBuilder<Transport> builder)
        {
            builder.HasData
            (
                new Transport
                {
                    Id = 1,
                    LoadCapacity = 1000,
                    RegistrationNumber = "A000AA",
                }
            );

            builder.OwnsOne(Transport => Transport.Driver).HasData
            (
                new //По возможности избавиться от анонимного типа
                {
                    TransportId = 1,
                    Name = "Sasha",
                    Surname = "Trikorochki",
                    Patronymic = "Vitaljevich",
                    PhoneNumber = "19(4235)386-91-39"
                }
            );
        }
    }
}
