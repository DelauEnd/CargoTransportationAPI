using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;

namespace CargoTransportationAPI.Formatters
{
    public static class FormatterSupportedTypes
    {
        public static readonly List<Type> SupportedTypes = new List<Type>
        {
            typeof(RouteDto),
            typeof(IEnumerable<RouteDto>),
            typeof(CargoDto),
            typeof(IEnumerable<CargoDto>),
            typeof(TransportDto),
            typeof(IEnumerable<TransportDto>)
        };
    }
}
