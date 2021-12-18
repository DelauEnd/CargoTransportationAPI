﻿using Entities.Models;

namespace CargoTransportationAPI.ActionFilters
{
    public class FilterAttribute
    {
        public Entity Entity { get; set; }

        public int EntityId { get; set; }

        public string EntityName { get; set; }
    }
}
