using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class OfferEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Offers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Offers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "text",
                nullable: true);
        }
    }
}
