using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using MiniCRMCore.Areas.Offers.Models;

namespace MiniCRMCore.Migrations
{
    public partial class OfferSectionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<OfferSectionType>>(
                name: "SelectedSections2",
                table: "Offers",
                type: "integer[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedSections2",
                table: "Offers");
        }
    }
}
