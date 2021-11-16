using AutoMapper;
using CargoTransportationAPI.Controllers;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.ObjectsForUpdate;
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
            CreateCargoMaps();
            CreateRouteMaps();
            CreateOrderMaps();
            CreateCustomerMaps();
            CreateCargoCategoryMaps();
        }

        private void CreateTransportMaps()
        {
            CreateMap<Transport, TransportDto>();

            CreateMap<TransportForCreation, Transport>();

            CreateMap<TransportForUpdate, Transport>().ReverseMap();
        }

        private void CreateCargoMaps()
        {
            CreateMap<Cargo, CargoDto>()
                .ForMember(cargoDto => cargoDto.Category, option =>
                option.MapFrom(cargo =>
                cargo.Category.Title));

            CreateMap<CargoForUpdate, Cargo>().ReverseMap();

            CreateMap<CargoForCreation, Cargo>();
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

            CreateMap<OrderForUpdate, Order>()
                .ForMember(order => order.Status, option =>
                option.MapFrom(order => 
                    Enum.IsDefined(typeof(EStatuses), order.Status) ?
                    Enum.Parse(typeof(EStatuses), order.Status) :
                    EStatuses.PROCESSING))
                .ReverseMap()
                .ForMember(updateOrder => updateOrder.Status, option  =>  
                option.MapFrom(order => order.Status.ToString()));
        }

        private void CreateCustomerMaps()
        {
            CreateMap<Customer, CustomerDto>();

            CreateMap<CustomerForCreation, Customer>();

            CreateMap<CustomerForUpdate, Customer>().ReverseMap();
        }

        private void CreateCargoCategoryMaps()
        {
            CreateMap<CargoCategory, CargoCategoryDto>();

            CreateMap<CategoryForCreation, CargoCategory>();

            CreateMap<CargoCategoryForUpdate, CargoCategory>().ReverseMap();
        }
    }
}
