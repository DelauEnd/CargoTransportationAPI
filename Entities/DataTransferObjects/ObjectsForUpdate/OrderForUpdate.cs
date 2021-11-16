using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.ObjectsForUpdate
{
    public class OrderForUpdate
    {
        public int SenderId { get; set; }

        public int DestinationId { get; set; }

        public string Status { get; set; }
    }
}
