using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class StudentBookingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "LogIn",
                newName: "Email");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "DormitoriesBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Dormitories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoriesBooking_StudentId",
                table: "DormitoriesBooking",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DormitoriesBooking_Students_StudentId",
                table: "DormitoriesBooking",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DormitoriesBooking_Students_StudentId",
                table: "DormitoriesBooking");

            migrationBuilder.DropIndex(
                name: "IX_DormitoriesBooking_StudentId",
                table: "DormitoriesBooking");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "DormitoriesBooking");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "LogIn",
                newName: "Username");

            migrationBuilder.AlterColumn<int>(
                name: "phone",
                table: "Dormitories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
