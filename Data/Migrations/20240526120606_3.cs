using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FkFiliale",
                table: "Compte",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Filiale",
                columns: table => new
                {
                    IdFiliale = table.Column<Guid>(nullable: false),
                    NomFiliale = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiale", x => x.IdFiliale);
                });

            migrationBuilder.CreateTable(
                name: "Atelier",
                columns: table => new
                {
                    IdAtelier = table.Column<Guid>(nullable: false),
                    NomAtelier = table.Column<string>(nullable: false),
                    FkFiliale = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atelier", x => x.IdAtelier);
                    table.ForeignKey(
                        name: "FkAtelier_Filiale_FkFiliale",
                        column: x => x.FkFiliale,
                        principalTable: "Filiale",
                        principalColumn: "IdFiliale",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    IdStation = table.Column<Guid>(nullable: false),
                    NomStation = table.Column<string>(nullable: false),
                    StationCode = table.Column<string>(nullable: true),
                    StationCapacity = table.Column<double>(nullable: false),
                    IsActif = table.Column<bool>(nullable: false),
                    TypeAmmortissement = table.Column<string>(nullable: true),
                    FkAtelier = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.IdStation);
                    table.ForeignKey(
                        name: "FkStation_Atelier_FkAtelier",
                        column: x => x.FkAtelier,
                        principalTable: "Atelier",
                        principalColumn: "IdAtelier",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compte_FkFiliale",
                table: "Compte",
                column: "FkFiliale");

            migrationBuilder.CreateIndex(
                name: "IX_Atelier_FkFiliale",
                table: "Atelier",
                column: "FkFiliale");

            migrationBuilder.CreateIndex(
                name: "IX_Station_FkAtelier",
                table: "Station",
                column: "FkAtelier");

            migrationBuilder.AddForeignKey(
                name: "FkCompte_Filiale_FkFiliale",
                table: "Compte",
                column: "FkFiliale",
                principalTable: "Filiale",
                principalColumn: "IdFiliale",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FkCompte_Filiale_FkFiliale",
                table: "Compte");

            migrationBuilder.DropTable(
                name: "Station");

            migrationBuilder.DropTable(
                name: "Atelier");

            migrationBuilder.DropTable(
                name: "Filiale");

            migrationBuilder.DropIndex(
                name: "IX_Compte_FkFiliale",
                table: "Compte");

            migrationBuilder.DropColumn(
                name: "FkFiliale",
                table: "Compte");
        }
    }
}
