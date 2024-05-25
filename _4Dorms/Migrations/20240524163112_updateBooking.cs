using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class updateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInDate",
                table: "DormitoriesBooking");

            migrationBuilder.DropColumn(
                name: "CheckOutDate",
                table: "DormitoriesBooking");

            migrationBuilder.RenameColumn(
                name: "privateRoom",
                table: "Rooms",
                newName: "PrivateRoom");

            migrationBuilder.RenameColumn(
                name: "NumOfprivateRooms",
                table: "Rooms",
                newName: "NumOfPrivateRooms");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "DormitoriesBooking",
                newName: "RoomType");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "PaymentGateways",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "DormitoriesBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "DormitoriesBooking");

            migrationBuilder.RenameColumn(
                name: "PrivateRoom",
                table: "Rooms",
                newName: "privateRoom");

            migrationBuilder.RenameColumn(
                name: "NumOfPrivateRooms",
                table: "Rooms",
                newName: "NumOfprivateRooms");

            migrationBuilder.RenameColumn(
                name: "RoomType",
                table: "DormitoriesBooking",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "CardNumber",
                table: "PaymentGateways",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInDate",
                table: "DormitoriesBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutDate",
                table: "DormitoriesBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
