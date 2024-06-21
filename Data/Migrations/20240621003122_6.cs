using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationEntretien_Station_StationIdStation",
                table: "StationEntretien");

            migrationBuilder.DropIndex(
                name: "IX_StationEntretien_StationIdStation",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "StationIdStation",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "CodeSourceEauEntretien",
                table: "SourceEauEntretien");

            migrationBuilder.AddColumn<string>(
                name: "NomSourceEauEntretien",
                table: "SourceEauEntretien",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationEntretien_FkStation",
                table: "StationEntretien",
                column: "FkStation");

            migrationBuilder.AddForeignKey(
                name: "FK_StationEntretien_Station_FkStation",
                table: "StationEntretien",
                column: "FkStation",
                principalTable: "Station",
                principalColumn: "IdStation",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationEntretien_Station_FkStation",
                table: "StationEntretien");

            migrationBuilder.DropIndex(
                name: "IX_StationEntretien_FkStation",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "NomSourceEauEntretien",
                table: "SourceEauEntretien");

            migrationBuilder.AddColumn<Guid>(
                name: "StationIdStation",
                table: "StationEntretien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeSourceEauEntretien",
                table: "SourceEauEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationEntretien_StationIdStation",
                table: "StationEntretien",
                column: "StationIdStation");

            migrationBuilder.AddForeignKey(
                name: "FK_StationEntretien_Station_StationIdStation",
                table: "StationEntretien",
                column: "StationIdStation",
                principalTable: "Station",
                principalColumn: "IdStation",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
