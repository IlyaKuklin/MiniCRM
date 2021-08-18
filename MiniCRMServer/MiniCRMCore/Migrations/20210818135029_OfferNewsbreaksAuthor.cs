using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class OfferNewsbreaksAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "OfferNewsbreaks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OfferNewsbreaks_AuthorId",
                table: "OfferNewsbreaks",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferNewsbreaks_Users_AuthorId",
                table: "OfferNewsbreaks",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferNewsbreaks_Users_AuthorId",
                table: "OfferNewsbreaks");

            migrationBuilder.DropIndex(
                name: "IX_OfferNewsbreaks_AuthorId",
                table: "OfferNewsbreaks");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "OfferNewsbreaks");
        }
    }
}
