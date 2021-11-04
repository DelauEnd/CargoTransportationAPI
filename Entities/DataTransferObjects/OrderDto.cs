using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderDto : IModelFormatter
    {
        public int Id { get; set; }

        public string Status { get; set; }


        public string Sender { get; set; }


        public string Destination { get; set; }


        public string FormatToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
