using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Dorms.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    AdministratorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.AdministratorId);
                });

            migrationBuilder.CreateTable(
                name: "DormitoryOwners",
                columns: table => new
                {
                    DormitoryOwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DormitoryOwners", x => x.DormitoryOwnerId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentGateways",
                columns: table => new
                {
                    PaymentGateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayerAccount = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGateways", x => x.PaymentGateId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Disabilities = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Dormitories",
                columns: table => new
                {
                    DormitoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DormitoryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DormitoryOwnerId = table.Column<int>(type: "int", nullable: true),
                    AdministratorId = table.Column<int>(type: "int", nullable: true),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dormitories", x => x.DormitoryId);
                    table.ForeignKey(
                        name: "FK_Dormitories_Administrators_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "Administrators",
                        principalColumn: "AdministratorId");
                    table.ForeignKey(
                        name: "FK_Dormitories_DormitoryOwners_DormitoryOwnerId",
                        column: x => x.DormitoryOwnerId,
                        principalTable: "DormitoryOwners",
                        principalColumn: "DormitoryOwnerId");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteLists",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    DormitoryOwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteLists", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_FavoriteLists_DormitoryOwners_DormitoryOwnerId",
                        column: x => x.DormitoryOwnerId,
                        principalTable: "DormitoryOwners",
                        principalColumn: "DormitoryOwnerId");
                    table.ForeignKey(
                        name: "FK_FavoriteLists_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    DormitoryId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId");
                    table.ForeignKey(
                        name: "FK_Reviews_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailabile = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    DormitoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId");
                });

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

            migrationBuilder.CreateTable(
                name: "DormitoriesBooking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    DormitoryId = table.Column<int>(type: "int", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DormitoryOwnerId = table.Column<int>(type: "int", nullable: true),
                    PaymentGateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DormitoriesBooking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_DormitoriesBooking_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId");
                    table.ForeignKey(
                        name: "FK_DormitoriesBooking_DormitoryOwners_DormitoryOwnerId",
                        column: x => x.DormitoryOwnerId,
                        principalTable: "DormitoryOwners",
                        principalColumn: "DormitoryOwnerId");
                    table.ForeignKey(
                        name: "FK_DormitoriesBooking_PaymentGateways_PaymentGateId",
                        column: x => x.PaymentGateId,
                        principalTable: "PaymentGateways",
                        principalColumn: "PaymentGateId");
                    table.ForeignKey(
                        name: "FK_DormitoriesBooking_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dormitories_AdministratorId",
                table: "Dormitories",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dormitories_DormitoryOwnerId",
                table: "Dormitories",
                column: "DormitoryOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoriesBooking_DormitoryId",
                table: "DormitoriesBooking",
                column: "DormitoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoriesBooking_DormitoryOwnerId",
                table: "DormitoriesBooking",
                column: "DormitoryOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoriesBooking_PaymentGateId",
                table: "DormitoriesBooking",
                column: "PaymentGateId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoriesBooking_RoomId",
                table: "DormitoriesBooking",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoryFavoriteList_FavoritesFavoriteId",
                table: "DormitoryFavoriteList",
                column: "FavoritesFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteLists_DormitoryOwnerId",
                table: "FavoriteLists",
                column: "DormitoryOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteLists_StudentId",
                table: "FavoriteLists",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DormitoryId",
                table: "Reviews",
                column: "DormitoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_StudentId",
                table: "Reviews",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DormitoryId",
                table: "Rooms",
                column: "DormitoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DormitoriesBooking");

            migrationBuilder.DropTable(
                name: "DormitoryFavoriteList");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "PaymentGateways");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "FavoriteLists");

            migrationBuilder.DropTable(
                name: "Dormitories");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "DormitoryOwners");
        }
    }
}
