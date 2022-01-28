using Entities.Models;
using System;

namespace Entities.DataTransferObjects
{
    public class CustomerDto : IModelFormatter
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public Person ContactPerson { get; set; }

        public string FormatToCsv()
        {
            var separator = ",\"";

            return string.Join
            (
                separator,
                Id,
                Address,
                ContactPerson.FormatToCsv()
            );
        }
    }
}
