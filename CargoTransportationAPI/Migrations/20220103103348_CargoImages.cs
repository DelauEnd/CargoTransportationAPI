using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CargoTransportationAPI.Migrations
{
    public partial class CargoImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d699a73-d8f8-49c6-b4e2-a8ba4d5a8126");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4459678-51b2-4e27-92a3-72e420052759");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Cargoes",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6cea63d5-83ee-4751-9ec3-e973b46d60cf", "50c22e9a-44c6-4e88-a820-36d692c45c50", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8bf62bc7-134b-4c2c-8b75-b4b34c6cc5f7", "44f8ec6f-b57c-4a08-a892-55ed37dff634", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.UpdateData(
                table: "Cargoes",
                keyColumn: "CargoId",
                keyValue: 1,
                columns: new[] { "ArrivalDate", "DepartureDate" },
                values: new object[] { new DateTime(2022, 1, 13, 13, 33, 47, 970, DateTimeKind.Local).AddTicks(8842), new DateTime(2022, 1, 3, 13, 33, 47, 970, DateTimeKind.Local).AddTicks(8602) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cea63d5-83ee-4751-9ec3-e973b46d60cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bf62bc7-134b-4c2c-8b75-b4b34c6cc5f7");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cargoes");

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
    }
}
