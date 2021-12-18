﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CargoTransportationAPI.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20211030125934_initialData")]
    partial class InitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Models.Cargo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CargoId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("RouteId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OrderId");

                    b.HasIndex("RouteId");

                    b.ToTable("Cargoes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArrivalDate = new DateTime(2021, 11, 9, 15, 59, 33, 645, DateTimeKind.Local).AddTicks(5243),
                            CategoryId = 1,
                            DepartureDate = new DateTime(2021, 10, 30, 15, 59, 33, 645, DateTimeKind.Local).AddTicks(4985),
                            OrderId = 1,
                            RouteId = 1,
                            Title = "Initial Cargo",
                            Weight = 200.0
                        });
                });

            modelBuilder.Entity("Entities.Models.CargoCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoryId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "Initial Category"
                        });
                });

            modelBuilder.Entity("Entities.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CustomerId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "14681 Longview Dr, Loxley, AL, 36551 "
                        });
                });

            modelBuilder.Entity("Entities.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("OrderId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DestinationId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DestinationId");

                    b.HasIndex("SenderId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DestinationId = 1,
                            SenderId = 1,
                            Status = 0
                        });
                });

            modelBuilder.Entity("Entities.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RouteId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TransportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransportId")
                        .IsUnique();

                    b.ToTable("Routes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            TransportId = 1
                        });
                });

            modelBuilder.Entity("Entities.Models.Transport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TransportId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("LoadCapacity")
                        .HasColumnType("float");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Transports");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LoadCapacity = 1000.0,
                            RegistrationNumber = "A000AA"
                        });
                });

            modelBuilder.Entity("Entities.Models.Cargo", b =>
                {
                    b.HasOne("Entities.Models.CargoCategory", "Category")
                        .WithMany("Cargoes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Order", "Order")
                        .WithMany("Cargoes")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Route", "Route")
                        .WithMany("Cargoes")
                        .HasForeignKey("RouteId");

                    b.OwnsOne("Entities.Models.Dimensions", "Dimensions", b1 =>
                        {
                            b1.Property<int>("CargoId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<double>("Height")
                                .HasColumnType("float");

                            b1.Property<double>("Length")
                                .HasColumnType("float");

                            b1.Property<double>("Width")
                                .HasColumnType("float");

                            b1.HasKey("CargoId");

                            b1.ToTable("Cargoes");

                            b1.WithOwner()
                                .HasForeignKey("CargoId");

                            b1.HasData(
                                new
                                {
                                    CargoId = 1,
                                    Height = 50.0,
                                    Length = 50.0,
                                    Width = 50.0
                                });
                        });

                    b.Navigation("Category");

                    b.Navigation("Dimensions")
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("Entities.Models.Customer", b =>
                {
                    b.OwnsOne("Entities.Models.Person", "ContactPerson", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Surname")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");

                            b1.HasData(
                                new
                                {
                                    CustomerId = 1,
                                    Name = "Pasha",
                                    Patronymic = "Olegovich",
                                    PhoneNumber = "86(4235)888-11-34",
                                    Surname = "Trikorochki"
                                });
                        });

                    b.Navigation("ContactPerson")
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Models.Order", b =>
                {
                    b.HasOne("Entities.Models.Customer", "Destination")
                        .WithMany("OrderDestination")
                        .HasForeignKey("DestinationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Entities.Models.Customer", "Sender")
                        .WithMany("OrderSender")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Destination");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Entities.Models.Route", b =>
                {
                    b.HasOne("Entities.Models.Transport", "Transport")
                        .WithOne("Route")
                        .HasForeignKey("Entities.Models.Route", "TransportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("Entities.Models.Transport", b =>
                {
                    b.OwnsOne("Entities.Models.Person", "Driver", b1 =>
                        {
                            b1.Property<int>("TransportId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.Property<string>("Surname")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)");

                            b1.HasKey("TransportId");

                            b1.ToTable("Transports");

                            b1.WithOwner()
                                .HasForeignKey("TransportId");

                            b1.HasData(
                                new
                                {
                                    TransportId = 1,
                                    Name = "Sasha",
                                    Patronymic = "Vitaljevich",
                                    PhoneNumber = "19(4235)386-91-39",
                                    Surname = "Trikorochki"
                                });
                        });

                    b.Navigation("Driver")
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Models.CargoCategory", b =>
                {
                    b.Navigation("Cargoes");
                });

            modelBuilder.Entity("Entities.Models.Customer", b =>
                {
                    b.Navigation("OrderDestination");

                    b.Navigation("OrderSender");
                });

            modelBuilder.Entity("Entities.Models.Order", b =>
                {
                    b.Navigation("Cargoes");
                });

            modelBuilder.Entity("Entities.Models.Route", b =>
                {
                    b.Navigation("Cargoes");
                });

            modelBuilder.Entity("Entities.Models.Transport", b =>
                {
                    b.Navigation("Route");
                });
#pragma warning restore 612, 618
        }
    }
}
