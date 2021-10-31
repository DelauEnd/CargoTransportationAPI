using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class RouteWithCargoesDto : IModelFormatter
    {
        public int Id { get; set; }

        public string TransportRegistrationNumber { get; set; }

        public List<CargoDto> Cargoes { get; set; }

        public string FormatToCsv()
        {
            var separator = ",\"";
            var cargoesCsvInfo = GetCargoInfo();

            return string.Join
                (
                separator,
                Id,
                TransportRegistrationNumber,
                cargoesCsvInfo
                );
        }

        private string GetCargoInfo()
        {
            var buffer = new StringBuilder();
            foreach (var cargo in Cargoes)
                buffer.Append(cargo.FormatToCsv());
            return buffer.ToString();
        }
    }
}
