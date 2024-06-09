using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class deleteDorm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DormitoriesBooking_Dormitories_DormitoryId",
                table: "DormitoriesBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Dormitories_DormitoryId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Dormitories_DormitoryId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "DormitoryFavoriteList");

            migrationBuilder.CreateTable(
                name: "FavoriteListDormitory",
                columns: table => new
                {
                    DormitoryId = table.Column<int>(type: "int", nullable: false),
                    FavoriteListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteListDormitory", x => new { x.DormitoryId, x.FavoriteListId });
                    table.ForeignKey(
                        name: "FK_FavoriteListDormitory_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteListDormitory_FavoriteLists_FavoriteListId",
                        column: x => x.FavoriteListId,
                        principalTable: "FavoriteLists",
                        principalColumn: "FavoriteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteListDormitory_FavoriteListId",
                table: "FavoriteListDormitory",
                column: "FavoriteListId");

            migrationBuilder.AddForeignKey(
                name: "FK_DormitoriesBooking_Dormitories_DormitoryId",
                table: "DormitoriesBooking",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "DormitoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Dormitories_DormitoryId",
                table: "Reviews",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "DormitoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Dormitories_DormitoryId",
                table: "Rooms",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "DormitoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DormitoriesBooking_Dormitories_DormitoryId",
                table: "DormitoriesBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Dormitories_DormitoryId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Dormitories_DormitoryId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "FavoriteListDormitory");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DormitoriesBooking_Dormitories_DormitoryId",
                table: "DormitoriesBooking",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "DormitoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Dormitories_DormitoryId",
                table: "Reviews",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "DormitoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Dormitories_DormitoryId",
                table: "Rooms",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "DormitoryId");
        }
    }
}
