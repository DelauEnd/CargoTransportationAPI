using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CargoTransportationAPI.Migrations
{
    public partial class AddedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c4459678-51b2-4e27-92a3-72e420052759", "b79635f8-5546-4136-90c8-b29a98345067", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8d699a73-d8f8-49c6-b4e2-a8ba4d5a8126", "3ceec481-1f98-4c78-9002-073f622c8b8a", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.UpdateData(
                table: "Cargoes",
                keyColumn: "CargoId",
                keyValue: 1,
                columns: new[] { "ArrivalDate", "DepartureDate" },
                values: new object[] { new DateTime(2021, 12, 1, 3, 57, 23, 103, DateTimeKind.Local).AddTicks(8588), new DateTime(2021, 11, 21, 3, 57, 23, 103, DateTimeKind.Local).AddTicks(8351) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d699a73-d8f8-49c6-b4e2-a8ba4d5a8126");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4459678-51b2-4e27-92a3-72e420052759");

            migrationBuilder.UpdateData(
                table: "Cargoes",
                keyColumn: "CargoId",
                keyValue: 1,
                columns: new[] { "ArrivalDate", "DepartureDate" },
                values: new object[] { new DateTime(2021, 12, 1, 3, 49, 16, 187, DateTimeKind.Local).AddTicks(9471), new DateTime(2021, 11, 21, 3, 49, 16, 187, DateTimeKind.Local).AddTicks(9227) });
        }
    }
}
