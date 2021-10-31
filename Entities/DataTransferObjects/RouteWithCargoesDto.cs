using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class RouteWithCargoesDto
    {
        public int Id { get; set; }

        public string TransportRegistrationNumber { get; set; }

        public List<CargoDto> Cargoes { get; set; }
    }
}
