using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class ClientLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientLink",
                table: "Offers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientLink",
                table: "Offers");
        }
    }
}
