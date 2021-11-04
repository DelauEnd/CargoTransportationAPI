using AutoMapper;
using CargoTransportationAPI.Controllers;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Enums;
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
            CreateCargoMap();
            CreateRouteMaps();
            CreateOrderMaps();
        }

        private void CreateOrderMaps()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(orderDto => orderDto.Sender, option => 
                option.MapFrom(order => order.Sender.Address))
                .ForMember(orderDto => orderDto.Destination, option => 
                option.MapFrom(order => order.Destination.Address));

            CreateMap<Order, OrderWithCargoesDto>()
                .ForMember(orderDto => orderDto.Sender, option =>
                option.MapFrom(order => order.Sender.Address))
                .ForMember(orderDto => orderDto.Destination, option =>
                option.MapFrom(order => order.Destination.Address));

            CreateMap<OrderForCreation, Order>()
                .ForMember(order => order.Status, option =>
                option.MapFrom(orderForCreation => EStatuses.PROCESSING));
        }

        private void CreateCargoMap()
        {
            CreateMap<Cargo, CargoDto>()
                .ForMember(cargoDto => cargoDto.Category, option =>
                option.MapFrom(cargo =>
                cargo.Category.Title));

            CreateMap<CargoForCreation, Cargo>();
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
