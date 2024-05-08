using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDormitoryStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPendingApproval",
                table: "Dormitories");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Dormitories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Dormitories");

            migrationBuilder.AddColumn<bool>(
                name: "IsPendingApproval",
                table: "Dormitories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
