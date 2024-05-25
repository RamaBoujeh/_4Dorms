using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class joinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DormitoryFavoriteList");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Dormitories",
                newName: "Phone");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteListFavoriteId",
                table: "Dormitories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DormitoryFavoriteLists",
                columns: table => new
                {
                    DormitoryId = table.Column<int>(type: "int", nullable: false),
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DormitoryFavoriteLists", x => new { x.DormitoryId, x.FavoriteId });
                    table.ForeignKey(
                        name: "FK_DormitoryFavoriteLists_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DormitoryFavoriteLists_FavoriteLists_FavoriteId",
                        column: x => x.FavoriteId,
                        principalTable: "FavoriteLists",
                        principalColumn: "FavoriteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dormitories_FavoriteListFavoriteId",
                table: "Dormitories",
                column: "FavoriteListFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoryFavoriteLists_FavoriteId",
                table: "DormitoryFavoriteLists",
                column: "FavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dormitories_FavoriteLists_FavoriteListFavoriteId",
                table: "Dormitories",
                column: "FavoriteListFavoriteId",
                principalTable: "FavoriteLists",
                principalColumn: "FavoriteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dormitories_FavoriteLists_FavoriteListFavoriteId",
                table: "Dormitories");

            migrationBuilder.DropTable(
                name: "DormitoryFavoriteLists");

            migrationBuilder.DropIndex(
                name: "IX_Dormitories_FavoriteListFavoriteId",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "FavoriteListFavoriteId",
                table: "Dormitories");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Dormitories",
                newName: "phone");

            migrationBuilder.CreateTable(
                name: "DormitoryFavoriteList",
                columns: table => new
                {
                    DormitoriesDormitoryId = table.Column<int>(type: "int", nullable: false),
                    FavoritesFavoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DormitoryFavoriteList", x => new { x.DormitoriesDormitoryId, x.FavoritesFavoriteId });
                    table.ForeignKey(
                        name: "FK_DormitoryFavoriteList_Dormitories_DormitoriesDormitoryId",
                        column: x => x.DormitoriesDormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DormitoryFavoriteList_FavoriteLists_FavoritesFavoriteId",
                        column: x => x.FavoritesFavoriteId,
                        principalTable: "FavoriteLists",
                        principalColumn: "FavoriteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DormitoryFavoriteList_FavoritesFavoriteId",
                table: "DormitoryFavoriteList",
                column: "FavoritesFavoriteId");
        }
    }
}
