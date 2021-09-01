using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class ClientVersionNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentVersion",
                table: "Offers",
                newName: "CurrentVersionNumber");

            migrationBuilder.AddColumn<int>(
                name: "ClientVersionNumber",
                table: "Offers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientVersionNumber",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "CurrentVersionNumber",
                table: "Offers",
                newName: "CurrentVersion");
        }
    }
}
