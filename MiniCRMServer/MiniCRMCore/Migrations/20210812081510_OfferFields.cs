using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class OfferFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Changed",
                table: "Offers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CoveringLetter",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Offers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsLinks",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferPoint",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocumentation",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Potential",
                table: "Offers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Recommendations",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SimilarCases",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "Offers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changed",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "CoveringLetter",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "NewsLinks",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OfferPoint",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OtherDocumentation",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Potential",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Recommendations",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "SimilarCases",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Offers");
        }
    }
}
