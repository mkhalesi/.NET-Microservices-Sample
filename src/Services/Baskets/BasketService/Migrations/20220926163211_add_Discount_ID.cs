using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BasketService.Migrations
{
    public partial class add_Discount_ID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "Baskets",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Baskets");
        }
    }
}
