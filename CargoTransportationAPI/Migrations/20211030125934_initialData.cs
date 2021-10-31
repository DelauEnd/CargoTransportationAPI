using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoTransportationAPI.Migrations
{
    public partial class initialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions_Long",
                table: "Cargoes");

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Surname",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_PhoneNumber",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Patronymic",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Name",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_Surname",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_PhoneNumber",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_Patronymic",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_Name",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dimensions_Width",
                table: "Cargoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Dimensions_Height",
                table: "Cargoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Dimensions_Length",
                table: "Cargoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Title" },
                values: new object[] { 1, "Initial Category" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactPerson_Name", "ContactPerson_Patronymic", "ContactPerson_PhoneNumber", "ContactPerson_Surname" },
                values: new object[] { 1, "14681 Longview Dr, Loxley, AL, 36551 ", "Pasha", "Olegovich", "86(4235)888-11-34", "Trikorochki" });

            migrationBuilder.InsertData(
                table: "Transports",
                columns: new[] { "TransportId", "LoadCapacity", "RegistrationNumber", "Driver_Name", "Driver_Patronymic", "Driver_PhoneNumber", "Driver_Surname" },
                values: new object[] { 1, 1000.0, "A000AA", "Sasha", "Vitaljevich", "19(4235)386-91-39", "Trikorochki" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "DestinationId", "SenderId", "Status" },
                values: new object[] { 1, 1, 1, 0 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "TransportId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Cargoes",
                columns: new[] { "CargoId", "ArrivalDate", "CategoryId", "DepartureDate", "OrderId", "RouteId", "Title", "Weight", "Dimensions_Height", "Dimensions_Length", "Dimensions_Width" },
                values: new object[] { 1, new DateTime(2021, 11, 9, 15, 59, 33, 645, DateTimeKind.Local).AddTicks(5243), 1, new DateTime(2021, 10, 30, 15, 59, 33, 645, DateTimeKind.Local).AddTicks(4985), 1, 1, "Initial Cargo", 200.0, 50.0, 50.0, 50.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cargoes",
                keyColumn: "CargoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumn: "RouteId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transports",
                keyColumn: "TransportId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Dimensions_Length",
                table: "Cargoes");

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Surname",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_PhoneNumber",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Patronymic",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Name",
                table: "Transports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_Surname",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_PhoneNumber",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_Patronymic",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson_Name",
                table: "Customers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<double>(
                name: "Dimensions_Width",
                table: "Cargoes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Dimensions_Height",
                table: "Cargoes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "Dimensions_Long",
                table: "Cargoes",
                type: "float",
                nullable: true);
        }
    }
}
