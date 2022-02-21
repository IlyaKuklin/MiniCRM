using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class CheckRemarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckRemark",
                table: "OfferRules",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheckStatus",
                table: "OfferRules",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckRemark",
                table: "OfferRules");

            migrationBuilder.DropColumn(
                name: "CheckStatus",
                table: "OfferRules");
        }
    }
}
