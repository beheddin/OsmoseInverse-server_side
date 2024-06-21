using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FournisseurIdFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropIndex(
                name: "IX_SourceEauEntretien_FournisseurIdFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "EntretienCharge",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "EntretienDescription",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "IsExternalEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "NomStationEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "FournisseurIdFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "NomBassin",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "NomPuit",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "SourceEauEntretienCharge",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "SourceEauEntretienDescription",
                table: "SourceEauEntretien");

            migrationBuilder.AddColumn<double>(
                name: "ChargeStationEntretien",
                table: "StationEntretien",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CodeStationEntretien",
                table: "StationEntretien",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionStationEntretien",
                table: "StationEntretien",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternalStationEntretien",
                table: "StationEntretien",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ChargeSourceEauEntretien",
                table: "SourceEauEntretien",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CodeSourceEauEntretien",
                table: "SourceEauEntretien",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionSourceEauEntretien",
                table: "SourceEauEntretien",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceEauEntretien_FkFournisseur",
                table: "SourceEauEntretien",
                column: "FkFournisseur");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FkFournisseur",
                table: "SourceEauEntretien",
                column: "FkFournisseur",
                principalTable: "Fournisseur",
                principalColumn: "IdFournisseur",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FkFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropIndex(
                name: "IX_SourceEauEntretien_FkFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "ChargeStationEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "CodeStationEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "DescriptionStationEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "IsExternalStationEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "ChargeSourceEauEntretien",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "CodeSourceEauEntretien",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "DescriptionSourceEauEntretien",
                table: "SourceEauEntretien");

            migrationBuilder.AddColumn<double>(
                name: "EntretienCharge",
                table: "StationEntretien",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "EntretienDescription",
                table: "StationEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternalEntretien",
                table: "StationEntretien",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NomStationEntretien",
                table: "StationEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FournisseurIdFournisseur",
                table: "SourceEauEntretien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomBassin",
                table: "SourceEauEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomPuit",
                table: "SourceEauEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SourceEauEntretienCharge",
                table: "SourceEauEntretien",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SourceEauEntretienDescription",
                table: "SourceEauEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceEauEntretien_FournisseurIdFournisseur",
                table: "SourceEauEntretien",
                column: "FournisseurIdFournisseur");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FournisseurIdFournisseur",
                table: "SourceEauEntretien",
                column: "FournisseurIdFournisseur",
                principalTable: "Fournisseur",
                principalColumn: "IdFournisseur",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
