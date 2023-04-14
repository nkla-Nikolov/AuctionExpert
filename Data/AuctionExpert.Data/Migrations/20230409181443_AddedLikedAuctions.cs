using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionExpert.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikedAuctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserAuction",
                columns: table => new
                {
                    LikedAuctionsId = table.Column<int>(type: "int", nullable: false),
                    UsersLikedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserAuction", x => new { x.LikedAuctionsId, x.UsersLikedId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserAuction_AspNetUsers_UsersLikedId",
                        column: x => x.UsersLikedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserAuction_Auctions_LikedAuctionsId",
                        column: x => x.LikedAuctionsId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserAuction_UsersLikedId",
                table: "ApplicationUserAuction",
                column: "UsersLikedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserAuction");
        }
    }
}
