using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderWithCargoesDto : IModelFormatter
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string Sender { get; set; }

        public string Destination { get; set; }

        public List<CargoDto> Cargoes { get; set; }

        public string FormatToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
