using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class TransportForCreation
    {
        public string RegistrationNumber { get; set; }

        public double LoadCapacity { get; set; }

        public Person Driver { get; set; }
    }
}
