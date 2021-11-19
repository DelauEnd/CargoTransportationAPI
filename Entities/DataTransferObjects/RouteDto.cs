using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class RouteDto : Dto, IModelFormatter
    {
        public int Id { get; set; }

        public string TransportRegistrationNumber { get; set; }

        public string FormatToCsv()
        {
            var separator = ",\"";

            return string.Join
                (
                separator,
                Id,
                TransportRegistrationNumber
                );
        }
    }
}
