using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FkFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "NomBassin",
                table: "SourceEau");

            migrationBuilder.DropColumn(
                name: "NomPuit",
                table: "SourceEau");

            migrationBuilder.AddColumn<string>(
                name: "NomSourceEau",
                table: "SourceEau",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FkFournisseur",
                table: "SourceEauEntretien",
                column: "FkFournisseur",
                principalTable: "Fournisseur",
                principalColumn: "IdFournisseur",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FkFournisseur",
                table: "SourceEauEntretien");

            migrationBuilder.DropColumn(
                name: "NomSourceEau",
                table: "SourceEau");

            migrationBuilder.AddColumn<string>(
                name: "NomBassin",
                table: "SourceEau",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomPuit",
                table: "SourceEau",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceEauEntretien_Fournisseur_FkFournisseur",
                table: "SourceEauEntretien",
                column: "FkFournisseur",
                principalTable: "Fournisseur",
                principalColumn: "IdFournisseur",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
