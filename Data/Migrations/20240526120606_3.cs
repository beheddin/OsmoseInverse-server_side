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
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Filiale",
                columns: table => new
                {
                    FilialeId = table.Column<Guid>(nullable: false),
                    FilialeLabel = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiale", x => x.FilialeId);
                });

            migrationBuilder.CreateTable(
                name: "Atelier",
                columns: table => new
                {
                    AtelierId = table.Column<Guid>(nullable: false),
                    AtelierLabel = table.Column<string>(nullable: false),
                    FkFiliale = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atelier", x => x.AtelierId);
                    table.ForeignKey(
                        name: "FK_Atelier_Filiale_FkFiliale",
                        column: x => x.FkFiliale,
                        principalTable: "Filiale",
                        principalColumn: "FilialeId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    StationId = table.Column<Guid>(nullable: false),
                    StationLabel = table.Column<string>(nullable: false),
                    StationCode = table.Column<string>(nullable: true),
                    StationCapacity = table.Column<double>(nullable: false),
                    IsActif = table.Column<bool>(nullable: false),
                    TypeAmmortissement = table.Column<string>(nullable: true),
                    FkAtelier = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.StationId);
                    table.ForeignKey(
                        name: "FK_Station_Atelier_FkAtelier",
                        column: x => x.FkAtelier,
                        principalTable: "Atelier",
                        principalColumn: "AtelierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_FkFiliale",
                table: "User",
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
                name: "FK_User_Filiale_FkFiliale",
                table: "User",
                column: "FkFiliale",
                principalTable: "Filiale",
                principalColumn: "FilialeId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Filiale_FkFiliale",
                table: "User");

            migrationBuilder.DropTable(
                name: "Station");

            migrationBuilder.DropTable(
                name: "Atelier");

            migrationBuilder.DropTable(
                name: "Filiale");

            migrationBuilder.DropIndex(
                name: "IX_User_FkFiliale",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FkFiliale",
                table: "User");
        }
    }
}
