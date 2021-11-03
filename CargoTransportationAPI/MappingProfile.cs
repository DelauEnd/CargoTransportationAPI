using AutoMapper;
using CargoTransportationAPI.Controllers;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
            CreateMap<TransportForCreation, Transport>();
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
