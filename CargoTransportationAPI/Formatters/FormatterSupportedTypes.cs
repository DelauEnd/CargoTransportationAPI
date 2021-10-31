using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Formatters
{
    public static class FormatterSupportedTypes
    {
        readonly public static List<Type> SupportedTypes = new List<Type>
        {
            typeof(RouteDto),
            typeof(IEnumerable<RouteDto>),
            typeof(RouteWithCargoesDto),
            typeof(IEnumerable<RouteWithCargoesDto>),
            typeof(CargoDto),
            typeof(IEnumerable<CargoDto>),
            typeof(TransportDto),
            typeof(IEnumerable<TransportDto>)
        };
    }
}
