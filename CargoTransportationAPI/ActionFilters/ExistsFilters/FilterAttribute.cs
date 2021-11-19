using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ActionFilters
{
    public class FilterAttribute
    {
        public Entity Entity { get; set; }
        
        public int EntityId { get; set; }

        public string EntityName { get; set; }
    }
}
