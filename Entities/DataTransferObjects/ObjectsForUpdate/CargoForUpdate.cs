using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.ObjectsForUpdate
{
    public class CargoForUpdate
    {
        public string Title { get; set; }

        public int CategoryId { get; set; }

        public DateTime DepartureDate { get; set; } = DateTime.Now;

        public DateTime ArrivalDate { get; set; } = DateTime.Now;

        public double Weight { get; set; }

        public Dimensions Dimensions { get; set; }
    }
}
