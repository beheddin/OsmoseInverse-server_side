using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ProduitConsommable");

            migrationBuilder.DropColumn(
                name: "Quantite",
                table: "ProduitConsommable");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInstallationProduitConsommable",
                table: "ProduitConsommable",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "QuantiteProduitConsommable",
                table: "ProduitConsommable",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateInstallationProduitConsommable",
                table: "ProduitConsommable");

            migrationBuilder.DropColumn(
                name: "QuantiteProduitConsommable",
                table: "ProduitConsommable");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ProduitConsommable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Quantite",
                table: "ProduitConsommable",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
