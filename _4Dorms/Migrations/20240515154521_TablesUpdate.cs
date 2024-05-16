using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class TablesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsAvailabile",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomNumber",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "PaymentGateways");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentGateways");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Rooms",
                newName: "RoomID");

            migrationBuilder.RenameColumn(
                name: "PayerAccount",
                table: "PaymentGateways",
                newName: "CardNumber");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Dormitories",
                newName: "PriceHalfYear");

            migrationBuilder.RenameColumn(
                name: "Amenities",
                table: "Dormitories",
                newName: "NearbyUniversity");

            migrationBuilder.AddColumn<int>(
                name: "NumOfSharedRooms",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumOfprivateRooms",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SharedRoom",
                table: "Rooms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "privateRoom",
                table: "Rooms",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CVV",
                table: "PaymentGateways",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaymentGateways",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Dormitories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DormitoryDescription",
                table: "Dormitories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Dormitories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GenderType",
                table: "Dormitories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceFullYear",
                table: "Dormitories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "phone",
                table: "Dormitories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfSharedRooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumOfprivateRooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SharedRoom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "privateRoom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CVV",
                table: "PaymentGateways");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PaymentGateways");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "DormitoryDescription",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "GenderType",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "PriceFullYear",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "Dormitories");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Rooms",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "CardNumber",
                table: "PaymentGateways",
                newName: "PayerAccount");

            migrationBuilder.RenameColumn(
                name: "PriceHalfYear",
                table: "Dormitories",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "NearbyUniversity",
                table: "Dormitories",
                newName: "Amenities");

            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailabile",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RoomNumber",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "PaymentGateways",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "PaymentGateways",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
