using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;

namespace CargoTransportationAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateTransportMaps();
            CreateOrderMap();
            CreateRouteMaps();
        }

        private void CreateOrderMap()
        {
            CreateMap<Cargo, CargoDto>()
                .ForMember(cargoDto => cargoDto.Category, option =>
                option.MapFrom(cargo =>
                cargo.Category.Title));
        }

        private void CreateTransportMaps()
        {
            CreateMap<Transport, TransportDto>();
        }

        private void CreateRouteMaps()
        {
            CreateMap<Route, RouteDto>()
                .ForMember(routeDto => routeDto.TransportRegistrationNumber, option =>
                option.MapFrom(transport =>
                transport.Transport.RegistrationNumber));

            CreateMap<Route, RouteWithCargoesDto>()
                .ForMember(routeDto => routeDto.TransportRegistrationNumber, option =>
                option.MapFrom(transport =>
                transport.Transport.RegistrationNumber))
                .ForMember(routeDto => routeDto.Cargoes, option =>
                option.MapFrom(Transport =>
                Transport.Cargoes));
        }
    }
}
