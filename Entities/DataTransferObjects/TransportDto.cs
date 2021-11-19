using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class TransportDto : Dto, IModelFormatter
    {
        public int Id { get; set; }

        public string RegistrationNumber { get; set; }

        public double LoadCapacity { get; set; }

        public Person Driver { get; set; }

        public string FormatToCsv()
        {
            var separator = ",\"";

            return string.Join
            (
                separator,
                Id,
                RegistrationNumber,
                LoadCapacity,
                Driver.Name,
                Driver.Surname,
                Driver.Patronymic,
                Driver.PhoneNumber
            );
        }
    }
}
