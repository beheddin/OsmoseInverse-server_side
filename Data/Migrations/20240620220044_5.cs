using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeStationEntretien",
                table: "StationEntretien");

            migrationBuilder.DropColumn(
                name: "CodeStation",
                table: "Station");

            migrationBuilder.DropColumn(
                name: "CodeFournisseur",
                table: "Fournisseur");

            migrationBuilder.AddColumn<string>(
                name: "NomStationEntretien",
                table: "StationEntretien",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomStationEntretien",
                table: "StationEntretien");

            migrationBuilder.AddColumn<string>(
                name: "CodeStationEntretien",
                table: "StationEntretien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeStation",
                table: "Station",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeFournisseur",
                table: "Fournisseur",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
