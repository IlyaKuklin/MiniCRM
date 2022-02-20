using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class OfferManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowedToViewAllOffers",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Offers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ManagerId",
                table: "Offers",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_ManagerId",
                table: "Offers",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_ManagerId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ManagerId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "AllowedToViewAllOffers",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Offers");
        }
    }
}
