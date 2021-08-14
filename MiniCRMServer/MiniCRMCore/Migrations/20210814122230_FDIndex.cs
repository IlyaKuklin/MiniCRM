using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class FDIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OfferFileData_FileDatumId",
                table: "OfferFileData");

            migrationBuilder.CreateIndex(
                name: "IX_OfferFileData_FileDatumId",
                table: "OfferFileData",
                column: "FileDatumId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OfferFileData_FileDatumId",
                table: "OfferFileData");

            migrationBuilder.CreateIndex(
                name: "IX_OfferFileData_FileDatumId",
                table: "OfferFileData",
                column: "FileDatumId");
        }
    }
}
