using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategorieProduitChimique",
                columns: table => new
                {
                    IdCategorie = table.Column<Guid>(nullable: false),
                    NomCategorie = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorieProduitChimique", x => x.IdCategorie);
                });

            migrationBuilder.CreateTable(
                name: "Filiale",
                columns: table => new
                {
                    IdFiliale = table.Column<Guid>(nullable: false),
                    NomFiliale = table.Column<string>(maxLength: 30, nullable: false),
                    AbbreviationNomFiliale = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiale", x => x.IdFiliale);
                });

            migrationBuilder.CreateTable(
                name: "Fournisseur",
                columns: table => new
                {
                    IdFournisseur = table.Column<Guid>(nullable: false),
                    NomFournisseur = table.Column<string>(nullable: true),
                    CodeFournisseur = table.Column<string>(nullable: true),
                    NumTelFournisseur = table.Column<int>(nullable: false),
                    NumFaxFournisseur = table.Column<int>(nullable: false),
                    EmailFournisseur = table.Column<string>(nullable: true),
                    AddresseFournisseur = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseur", x => x.IdFournisseur);
                });

            migrationBuilder.CreateTable(
                name: "NatureEquipment",
                columns: table => new
                {
                    IdNatureEquipment = table.Column<Guid>(nullable: false),
                    LabelNatureEquipment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NatureEquipment", x => x.IdNatureEquipment);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(nullable: false),
                    NomRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "SuiviType",
                columns: table => new
                {
                    IdSuiviType = table.Column<Guid>(nullable: false),
                    LabelSuiviType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuiviType", x => x.IdSuiviType);
                });

            migrationBuilder.CreateTable(
                name: "TypeEquipment",
                columns: table => new
                {
                    IdTypeEquipment = table.Column<Guid>(nullable: false),
                    LabelTypeEquipment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEquipment", x => x.IdTypeEquipment);
                });

            migrationBuilder.CreateTable(
                name: "Unite",
                columns: table => new
                {
                    IdUnite = table.Column<Guid>(nullable: false),
                    LabelUnite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unite", x => x.IdUnite);
                });

            migrationBuilder.CreateTable(
                name: "ProduitChimique",
                columns: table => new
                {
                    IdProduitChimique = table.Column<Guid>(nullable: false),
                    LabelProduitChimique = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    FkCategorieProduitChimique = table.Column<Guid>(nullable: true),
                    CategorieProduitChimiqueIdCategorie = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduitChimique", x => x.IdProduitChimique);
                    table.ForeignKey(
                        name: "FK_ProduitChimique_CategorieProduitChimique_CategorieProduitChimiqueIdCategorie",
                        column: x => x.CategorieProduitChimiqueIdCategorie,
                        principalTable: "CategorieProduitChimique",
                        principalColumn: "IdCategorie",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atelier",
                columns: table => new
                {
                    IdAtelier = table.Column<Guid>(nullable: false),
                    NomAtelier = table.Column<string>(maxLength: 30, nullable: false),
                    FkFiliale = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atelier", x => x.IdAtelier);
                    table.ForeignKey(
                        name: "FK_Atelier_Filiale_FkFiliale",
                        column: x => x.FkFiliale,
                        principalTable: "Filiale",
                        principalColumn: "IdFiliale",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SourceEau",
                columns: table => new
                {
                    IdSourceEau = table.Column<Guid>(nullable: false),
                    VolumeEau = table.Column<double>(nullable: false),
                    FkFiliale = table.Column<Guid>(nullable: true),
                    Descriminant = table.Column<string>(nullable: false),
                    NomBassin = table.Column<string>(maxLength: 30, nullable: true),
                    NomPuit = table.Column<string>(maxLength: 30, nullable: true),
                    Profondeur = table.Column<double>(nullable: true),
                    TypeAmortissement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceEau", x => x.IdSourceEau);
                    table.ForeignKey(
                        name: "FK_SourceEau_Filiale_FkFiliale",
                        column: x => x.FkFiliale,
                        principalTable: "Filiale",
                        principalColumn: "IdFiliale",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Compte",
                columns: table => new
                {
                    IdCompte = table.Column<Guid>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    CIN = table.Column<string>(maxLength: 8, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Access = table.Column<bool>(nullable: false),
                    FkRole = table.Column<Guid>(nullable: true),
                    FkFiliale = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compte", x => x.IdCompte);
                    table.ForeignKey(
                        name: "FK_Compte_Filiale_FkFiliale",
                        column: x => x.FkFiliale,
                        principalTable: "Filiale",
                        principalColumn: "IdFiliale",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Compte_Role_FkRole",
                        column: x => x.FkRole,
                        principalTable: "Role",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SuiviParametre",
                columns: table => new
                {
                    IdSuiviParametre = table.Column<Guid>(nullable: false),
                    LabelSuiviParametre = table.Column<string>(nullable: true),
                    FkSuiviType = table.Column<Guid>(nullable: true),
                    SuiviTypeIdSuiviType = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuiviParametre", x => x.IdSuiviParametre);
                    table.ForeignKey(
                        name: "FK_SuiviParametre_SuiviType_SuiviTypeIdSuiviType",
                        column: x => x.SuiviTypeIdSuiviType,
                        principalTable: "SuiviType",
                        principalColumn: "IdSuiviType",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    IdStation = table.Column<Guid>(nullable: false),
                    NomStation = table.Column<string>(maxLength: 30, nullable: false),
                    CodeStation = table.Column<string>(nullable: true),
                    CapaciteStation = table.Column<int>(nullable: false),
                    TypeAmmortissement = table.Column<string>(nullable: true),
                    IsActif = table.Column<bool>(nullable: false),
                    FkAtelier = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.IdStation);
                    table.ForeignKey(
                        name: "FK_Station_Atelier_FkAtelier",
                        column: x => x.FkAtelier,
                        principalTable: "Atelier",
                        principalColumn: "IdAtelier",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SourceEauEntretien",
                columns: table => new
                {
                    IdSourceEauEntretien = table.Column<Guid>(nullable: false),
                    SourceEauEntretienDescription = table.Column<string>(nullable: true),
                    SourceEauEntretienCharge = table.Column<double>(nullable: false),
                    IsExternalSourceEauEntretien = table.Column<bool>(nullable: false),
                    Descriminant = table.Column<string>(nullable: true),
                    FkSourceEau = table.Column<Guid>(nullable: true),
                    NomPuit = table.Column<string>(nullable: true),
                    NomBassin = table.Column<string>(nullable: true),
                    FkFournisseur = table.Column<Guid>(nullable: true),
                    FournisseurIdFournisseur = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceEauEntretien", x => x.IdSourceEauEntretien);
                    table.ForeignKey(
                        name: "FK_SourceEauEntretien_SourceEau_FkSourceEau",
                        column: x => x.FkSourceEau,
                        principalTable: "SourceEau",
                        principalColumn: "IdSourceEau",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SourceEauEntretien_Fournisseur_FournisseurIdFournisseur",
                        column: x => x.FournisseurIdFournisseur,
                        principalTable: "Fournisseur",
                        principalColumn: "IdFournisseur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    IdEquipment = table.Column<Guid>(nullable: false),
                    LabelEquipment = table.Column<string>(nullable: true),
                    FkStation = table.Column<Guid>(nullable: true),
                    StationIdStation = table.Column<Guid>(nullable: true),
                    FkNatureEquipment = table.Column<Guid>(nullable: true),
                    NatureEquipmentIdNatureEquipment = table.Column<Guid>(nullable: true),
                    FkTypeEquipment = table.Column<Guid>(nullable: true),
                    TypeEquipmentIdTypeEquipment = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.IdEquipment);
                    table.ForeignKey(
                        name: "FK_Equipment_NatureEquipment_NatureEquipmentIdNatureEquipment",
                        column: x => x.NatureEquipmentIdNatureEquipment,
                        principalTable: "NatureEquipment",
                        principalColumn: "IdNatureEquipment",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Station_StationIdStation",
                        column: x => x.StationIdStation,
                        principalTable: "Station",
                        principalColumn: "IdStation",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_TypeEquipment_TypeEquipmentIdTypeEquipment",
                        column: x => x.TypeEquipmentIdTypeEquipment,
                        principalTable: "TypeEquipment",
                        principalColumn: "IdTypeEquipment",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LavageChimique",
                columns: table => new
                {
                    IdLavageChimique = table.Column<Guid>(nullable: false),
                    DateLavageChimique = table.Column<DateTime>(nullable: false),
                    FkProduitChimique = table.Column<Guid>(nullable: true),
                    ProduitChimiqueIdProduitChimique = table.Column<Guid>(nullable: true),
                    FkStation = table.Column<Guid>(nullable: true),
                    StationIdStation = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LavageChimique", x => x.IdLavageChimique);
                    table.ForeignKey(
                        name: "FK_LavageChimique_ProduitChimique_ProduitChimiqueIdProduitChimique",
                        column: x => x.ProduitChimiqueIdProduitChimique,
                        principalTable: "ProduitChimique",
                        principalColumn: "IdProduitChimique",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LavageChimique_Station_StationIdStation",
                        column: x => x.StationIdStation,
                        principalTable: "Station",
                        principalColumn: "IdStation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Objectif",
                columns: table => new
                {
                    IdObjectif = table.Column<Guid>(nullable: false),
                    Annee = table.Column<int>(nullable: false),
                    TDSeauBrute = table.Column<double>(nullable: false),
                    TDSeauOsmosee = table.Column<double>(nullable: false),
                    RendementMembranes = table.Column<double>(nullable: false),
                    Rendement = table.Column<double>(nullable: false),
                    PermanentFlow = table.Column<double>(nullable: false),
                    TauxExploitation = table.Column<double>(nullable: false),
                    DeltapMicrofilter = table.Column<double>(nullable: false),
                    DeltapMembrane = table.Column<double>(nullable: false),
                    DeltapSable = table.Column<double>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    TH = table.Column<double>(nullable: false),
                    PH = table.Column<double>(nullable: false),
                    TauxChlorure = table.Column<double>(nullable: false),
                    FkStation = table.Column<Guid>(nullable: true),
                    StationIdStation = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectif", x => x.IdObjectif);
                    table.ForeignKey(
                        name: "FK_Objectif_Station_StationIdStation",
                        column: x => x.StationIdStation,
                        principalTable: "Station",
                        principalColumn: "IdStation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProduitConsommable",
                columns: table => new
                {
                    IdProduitConsommable = table.Column<Guid>(nullable: false),
                    LabelProduitConsommable = table.Column<string>(nullable: true),
                    QuantiteProduitConsommable = table.Column<double>(nullable: false),
                    DateInstallationProduitConsommable = table.Column<DateTime>(nullable: false),
                    FkStation = table.Column<Guid>(nullable: true),
                    StationIdStation = table.Column<Guid>(nullable: true),
                    FkUnite = table.Column<Guid>(nullable: true),
                    UniteIdUnite = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ConfigurationPompe = table.Column<string>(nullable: true),
                    ConcentrationDosageChimique = table.Column<double>(nullable: true),
                    FkProduitChimique = table.Column<Guid>(nullable: true),
                    ProduitChimiqueIdProduitChimique = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduitConsommable", x => x.IdProduitConsommable);
                    table.ForeignKey(
                        name: "FK_ProduitConsommable_ProduitChimique_ProduitChimiqueIdProduitChimique",
                        column: x => x.ProduitChimiqueIdProduitChimique,
                        principalTable: "ProduitChimique",
                        principalColumn: "IdProduitChimique",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProduitConsommable_Station_StationIdStation",
                        column: x => x.StationIdStation,
                        principalTable: "Station",
                        principalColumn: "IdStation",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProduitConsommable_Unite_UniteIdUnite",
                        column: x => x.UniteIdUnite,
                        principalTable: "Unite",
                        principalColumn: "IdUnite",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StationEntretien",
                columns: table => new
                {
                    IdStationEntretien = table.Column<Guid>(nullable: false),
                    NomStationEntretien = table.Column<string>(nullable: true),
                    EntretienDescription = table.Column<string>(nullable: true),
                    EntretienCharge = table.Column<double>(nullable: false),
                    IsExternalEntretien = table.Column<bool>(nullable: false),
                    FkStation = table.Column<Guid>(nullable: true),
                    StationIdStation = table.Column<Guid>(nullable: true),
                    FkFournisseur = table.Column<Guid>(nullable: true),
                    FournisseurIdFournisseur = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationEntretien", x => x.IdStationEntretien);
                    table.ForeignKey(
                        name: "FK_StationEntretien_Fournisseur_FournisseurIdFournisseur",
                        column: x => x.FournisseurIdFournisseur,
                        principalTable: "Fournisseur",
                        principalColumn: "IdFournisseur",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StationEntretien_Station_StationIdStation",
                        column: x => x.StationIdStation,
                        principalTable: "Station",
                        principalColumn: "IdStation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StationParametre",
                columns: table => new
                {
                    IdStationParametre = table.Column<Guid>(nullable: false),
                    FkStation = table.Column<Guid>(nullable: true),
                    StationIdStation = table.Column<Guid>(nullable: true),
                    FkSuiviParametre = table.Column<Guid>(nullable: true),
                    SuiviParametreIdSuiviParametre = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationParametre", x => x.IdStationParametre);
                    table.ForeignKey(
                        name: "FK_StationParametre_Station_StationIdStation",
                        column: x => x.StationIdStation,
                        principalTable: "Station",
                        principalColumn: "IdStation",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StationParametre_SuiviParametre_SuiviParametreIdSuiviParametre",
                        column: x => x.SuiviParametreIdSuiviParametre,
                        principalTable: "SuiviParametre",
                        principalColumn: "IdSuiviParametre",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuiviQuotidien",
                columns: table => new
                {
                    IdSuiviQuotidien = table.Column<Guid>(nullable: false),
                    DateSuiviQuotidien = table.Column<DateTime>(nullable: false),
                    ValueSuiviQuotidien = table.Column<double>(nullable: false),
                    FkStationParametre = table.Column<Guid>(nullable: true),
                    StationParametreIdStationParametre = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuiviQuotidien", x => x.IdSuiviQuotidien);
                    table.ForeignKey(
                        name: "FK_SuiviQuotidien_StationParametre_StationParametreIdStationParametre",
                        column: x => x.StationParametreIdStationParametre,
                        principalTable: "StationParametre",
                        principalColumn: "IdStationParametre",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atelier_FkFiliale",
                table: "Atelier",
                column: "FkFiliale");

            migrationBuilder.CreateIndex(
                name: "IX_Compte_FkFiliale",
                table: "Compte",
                column: "FkFiliale");

            migrationBuilder.CreateIndex(
                name: "IX_Compte_FkRole",
                table: "Compte",
                column: "FkRole");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_NatureEquipmentIdNatureEquipment",
                table: "Equipment",
                column: "NatureEquipmentIdNatureEquipment");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_StationIdStation",
                table: "Equipment",
                column: "StationIdStation");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_TypeEquipmentIdTypeEquipment",
                table: "Equipment",
                column: "TypeEquipmentIdTypeEquipment");

            migrationBuilder.CreateIndex(
                name: "IX_LavageChimique_ProduitChimiqueIdProduitChimique",
                table: "LavageChimique",
                column: "ProduitChimiqueIdProduitChimique");

            migrationBuilder.CreateIndex(
                name: "IX_LavageChimique_StationIdStation",
                table: "LavageChimique",
                column: "StationIdStation");

            migrationBuilder.CreateIndex(
                name: "IX_Objectif_StationIdStation",
                table: "Objectif",
                column: "StationIdStation");

            migrationBuilder.CreateIndex(
                name: "IX_ProduitChimique_CategorieProduitChimiqueIdCategorie",
                table: "ProduitChimique",
                column: "CategorieProduitChimiqueIdCategorie");

            migrationBuilder.CreateIndex(
                name: "IX_ProduitConsommable_ProduitChimiqueIdProduitChimique",
                table: "ProduitConsommable",
                column: "ProduitChimiqueIdProduitChimique");

            migrationBuilder.CreateIndex(
                name: "IX_ProduitConsommable_StationIdStation",
                table: "ProduitConsommable",
                column: "StationIdStation");

            migrationBuilder.CreateIndex(
                name: "IX_ProduitConsommable_UniteIdUnite",
                table: "ProduitConsommable",
                column: "UniteIdUnite");

            migrationBuilder.CreateIndex(
                name: "IX_SourceEau_FkFiliale",
                table: "SourceEau",
                column: "FkFiliale");

            migrationBuilder.CreateIndex(
                name: "IX_SourceEauEntretien_FkSourceEau",
                table: "SourceEauEntretien",
                column: "FkSourceEau");

            migrationBuilder.CreateIndex(
                name: "IX_SourceEauEntretien_FournisseurIdFournisseur",
                table: "SourceEauEntretien",
                column: "FournisseurIdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_Station_FkAtelier",
                table: "Station",
                column: "FkAtelier");

            migrationBuilder.CreateIndex(
                name: "IX_StationEntretien_FournisseurIdFournisseur",
                table: "StationEntretien",
                column: "FournisseurIdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_StationEntretien_StationIdStation",
                table: "StationEntretien",
                column: "StationIdStation");

            migrationBuilder.CreateIndex(
                name: "IX_StationParametre_StationIdStation",
                table: "StationParametre",
                column: "StationIdStation");

            migrationBuilder.CreateIndex(
                name: "IX_StationParametre_SuiviParametreIdSuiviParametre",
                table: "StationParametre",
                column: "SuiviParametreIdSuiviParametre");

            migrationBuilder.CreateIndex(
                name: "IX_SuiviParametre_SuiviTypeIdSuiviType",
                table: "SuiviParametre",
                column: "SuiviTypeIdSuiviType");

            migrationBuilder.CreateIndex(
                name: "IX_SuiviQuotidien_StationParametreIdStationParametre",
                table: "SuiviQuotidien",
                column: "StationParametreIdStationParametre");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compte");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "LavageChimique");

            migrationBuilder.DropTable(
                name: "Objectif");

            migrationBuilder.DropTable(
                name: "ProduitConsommable");

            migrationBuilder.DropTable(
                name: "SourceEauEntretien");

            migrationBuilder.DropTable(
                name: "StationEntretien");

            migrationBuilder.DropTable(
                name: "SuiviQuotidien");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "NatureEquipment");

            migrationBuilder.DropTable(
                name: "TypeEquipment");

            migrationBuilder.DropTable(
                name: "ProduitChimique");

            migrationBuilder.DropTable(
                name: "Unite");

            migrationBuilder.DropTable(
                name: "SourceEau");

            migrationBuilder.DropTable(
                name: "Fournisseur");

            migrationBuilder.DropTable(
                name: "StationParametre");

            migrationBuilder.DropTable(
                name: "CategorieProduitChimique");

            migrationBuilder.DropTable(
                name: "Station");

            migrationBuilder.DropTable(
                name: "SuiviParametre");

            migrationBuilder.DropTable(
                name: "Atelier");

            migrationBuilder.DropTable(
                name: "SuiviType");

            migrationBuilder.DropTable(
                name: "Filiale");
        }
    }
}
