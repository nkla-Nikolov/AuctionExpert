using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionExpert.Data.Migrations
{
    public partial class AddedDatePlacedOnReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePlaced",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePlaced",
                table: "Reviews");
        }
    }
}
