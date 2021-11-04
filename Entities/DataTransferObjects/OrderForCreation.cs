using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderForCreation
    {
        public int SenderId { get; set; }

        public int DestinationId { get; set; }

        public IEnumerable<CargoForCreation> Cargoes { get; set; }
    }
}
