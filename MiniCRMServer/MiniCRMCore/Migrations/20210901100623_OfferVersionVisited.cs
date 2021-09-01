using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class OfferVersionVisited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisitedByClient",
                table: "OfferVersions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitedByClient",
                table: "OfferVersions");
        }
    }
}
