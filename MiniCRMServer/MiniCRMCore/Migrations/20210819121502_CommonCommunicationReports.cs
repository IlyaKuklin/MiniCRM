using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MiniCRMCore.Migrations
{
    public partial class CommonCommunicationReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCommunicationReports_Clients_ClientId",
                table: "ClientCommunicationReports");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "ClientCommunicationReports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "CommunicationReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: true),
                    OfferId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationReports_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationReports_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationReports_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationReports_AuthorId",
                table: "CommunicationReports",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationReports_ClientId",
                table: "CommunicationReports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationReports_OfferId",
                table: "CommunicationReports",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCommunicationReports_Clients_ClientId",
                table: "ClientCommunicationReports",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCommunicationReports_Clients_ClientId",
                table: "ClientCommunicationReports");

            migrationBuilder.DropTable(
                name: "CommunicationReports");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "ClientCommunicationReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCommunicationReports_Clients_ClientId",
                table: "ClientCommunicationReports",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
