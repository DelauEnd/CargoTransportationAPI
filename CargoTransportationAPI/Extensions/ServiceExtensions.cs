﻿using CargoTransportationAPI.Formatters;
using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Repository;

namespace CargoTransportationAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
            => services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        public static void ConfigureIISIntegration(this IServiceCollection services)
            => services.Configure<IISOptions>(options =>
            {
            });

        public static void ConfigureLoggerService(this IServiceCollection services)
            => services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection"), x =>
            x.MigrationsAssembly("CargoTransportationAPI")));

        public static void ConfigureRepositoryManager(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureFormatters(this IMvcBuilder builder)
            => builder
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddXmlDataContractSerializerFormatters()
            .AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
            => services
            .Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }
}
