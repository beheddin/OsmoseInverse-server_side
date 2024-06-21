using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationEntretien_Fournisseur_FournisseurIdFournisseur",
                table: "StationEntretien");

            migrationBuilder.DropIndex(
                name: "IX_StationEntretien_FournisseurIdFournisseur",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "FournisseurIdFournisseur",
                table: "StationEntretien");

            migrationBuilder.CreateIndex(
                name: "IX_StationEntretien_FkFournisseur",
                table: "StationEntretien",
                column: "FkFournisseur");

            migrationBuilder.AddForeignKey(
                name: "FK_StationEntretien_Fournisseur_FkFournisseur",
                table: "StationEntretien",
                column: "FkFournisseur",
                principalTable: "Fournisseur",
                principalColumn: "IdFournisseur",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationEntretien_Fournisseur_FkFournisseur",
                table: "StationEntretien");

            migrationBuilder.DropIndex(
                name: "IX_StationEntretien_FkFournisseur",
                table: "StationEntretien");

            migrationBuilder.AddColumn<Guid>(
                name: "FournisseurIdFournisseur",
                table: "StationEntretien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationEntretien_FournisseurIdFournisseur",
                table: "StationEntretien",
                column: "FournisseurIdFournisseur");

            migrationBuilder.AddForeignKey(
                name: "FK_StationEntretien_Fournisseur_FournisseurIdFournisseur",
                table: "StationEntretien",
                column: "FournisseurIdFournisseur",
                principalTable: "Fournisseur",
                principalColumn: "IdFournisseur",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
