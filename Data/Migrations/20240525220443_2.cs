using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FkCompte_Role_FkRole",
                table: "Compte");

            migrationBuilder.AlterColumn<Guid>(
                name: "FkRole",
                table: "Compte",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FkCompte_Role_FkRole",
                table: "Compte",
                column: "FkRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FkCompte_Role_FkRole",
                table: "Compte");

            migrationBuilder.AlterColumn<Guid>(
                name: "FkRole",
                table: "Compte",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FkCompte_Role_FkRole",
                table: "Compte",
                column: "FkRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
