using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class seedingAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "AdministratorId", "Email", "Name", "Password", "PhoneNumber" },
                values: new object[] { 1, "Ruaa@example.com", "Ruaa", "000", "1234567890" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "AdministratorId",
                keyValue: 1);
        }
    }
}
