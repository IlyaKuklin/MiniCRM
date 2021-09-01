using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCRMCore.Migrations
{
    public partial class FeedbackRequestFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "OfferFeedbackRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Answered",
                table: "OfferFeedbackRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "OfferFeedbackRequests");

            migrationBuilder.DropColumn(
                name: "Answered",
                table: "OfferFeedbackRequests");
        }
    }
}
