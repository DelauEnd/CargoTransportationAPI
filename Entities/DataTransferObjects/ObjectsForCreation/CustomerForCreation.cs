using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CustomerForCreation
    {
        public string Address { get; set; }

        public Person ContactPerson { get; set; }
    }
}
