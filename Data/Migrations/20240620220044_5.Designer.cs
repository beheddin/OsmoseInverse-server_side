﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(OsmoseInverseDbContext))]
    [Migration("20240620220044_5")]
    partial class _5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Models.Atelier", b =>
                {
                    b.Property<Guid>("IdAtelier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkFiliale")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NomAtelier")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("IdAtelier");

                    b.HasIndex("FkFiliale");

                    b.ToTable("Atelier");
                });

            modelBuilder.Entity("Domain.Models.CategorieProduitChimique", b =>
                {
                    b.Property<Guid>("IdCategorie")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NomCategorie")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("IdCategorie");

                    b.ToTable("CategorieProduitChimique");
                });

            modelBuilder.Entity("Domain.Models.Compte", b =>
                {
                    b.Property<Guid>("IdCompte")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Access")
                        .HasColumnType("bit");

                    b.Property<string>("CIN")
                        .IsRequired()
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.Property<Guid?>("FkFiliale")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkRole")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCompte");

                    b.HasIndex("FkFiliale");

                    b.HasIndex("FkRole");

                    b.ToTable("Compte");
                });

            modelBuilder.Entity("Domain.Models.Equipment", b =>
                {
                    b.Property<Guid>("IdEquipment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkNatureEquipment")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkTypeEquipment")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelEquipment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("NatureEquipmentIdNatureEquipment")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StationIdStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TypeEquipmentIdTypeEquipment")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdEquipment");

                    b.HasIndex("NatureEquipmentIdNatureEquipment");

                    b.HasIndex("StationIdStation");

                    b.HasIndex("TypeEquipmentIdTypeEquipment");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("Domain.Models.Filiale", b =>
                {
                    b.Property<Guid>("IdFiliale")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AbbreviationNomFiliale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomFiliale")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("IdFiliale");

                    b.ToTable("Filiale");
                });

            modelBuilder.Entity("Domain.Models.Fournisseur", b =>
                {
                    b.Property<Guid>("IdFournisseur")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddresseFournisseur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailFournisseur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomFournisseur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumFaxFournisseur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumTelFournisseur")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdFournisseur");

                    b.ToTable("Fournisseur");
                });

            modelBuilder.Entity("Domain.Models.LavageChimique", b =>
                {
                    b.Property<Guid>("IdLavageChimique")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateLavageChimique")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FkProduitChimique")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProduitChimiqueIdProduitChimique")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StationIdStation")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdLavageChimique");

                    b.HasIndex("ProduitChimiqueIdProduitChimique");

                    b.HasIndex("StationIdStation");

                    b.ToTable("LavageChimique");
                });

            modelBuilder.Entity("Domain.Models.NatureEquipment", b =>
                {
                    b.Property<Guid>("IdNatureEquipment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelNatureEquipment")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdNatureEquipment");

                    b.ToTable("NatureEquipment");
                });

            modelBuilder.Entity("Domain.Models.Objectif", b =>
                {
                    b.Property<Guid>("IdObjectif")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Annee")
                        .HasColumnType("int");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<double>("DeltapMembrane")
                        .HasColumnType("float");

                    b.Property<double>("DeltapMicrofilter")
                        .HasColumnType("float");

                    b.Property<double>("DeltapSable")
                        .HasColumnType("float");

                    b.Property<Guid?>("FkStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("PH")
                        .HasColumnType("float");

                    b.Property<double>("PermanentFlow")
                        .HasColumnType("float");

                    b.Property<double>("Rendement")
                        .HasColumnType("float");

                    b.Property<double>("RendementMembranes")
                        .HasColumnType("float");

                    b.Property<Guid?>("StationIdStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("TDSeauBrute")
                        .HasColumnType("float");

                    b.Property<double>("TDSeauOsmosee")
                        .HasColumnType("float");

                    b.Property<double>("TH")
                        .HasColumnType("float");

                    b.Property<double>("TauxChlorure")
                        .HasColumnType("float");

                    b.Property<double>("TauxExploitation")
                        .HasColumnType("float");

                    b.HasKey("IdObjectif");

                    b.HasIndex("StationIdStation");

                    b.ToTable("Objectif");
                });

            modelBuilder.Entity("Domain.Models.ProduitChimique", b =>
                {
                    b.Property<Guid>("IdProduitChimique")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CategorieProduitChimiqueIdCategorie")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkCategorieProduitChimique")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelProduitChimique")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdProduitChimique");

                    b.HasIndex("CategorieProduitChimiqueIdCategorie");

                    b.ToTable("ProduitChimique");
                });

            modelBuilder.Entity("Domain.Models.ProduitConsommable", b =>
                {
                    b.Property<Guid>("IdProduitConsommable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateInstallationProduitConsommable")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FkStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkUnite")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelProduitConsommable")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("QuantiteProduitConsommable")
                        .HasColumnType("float");

                    b.Property<Guid?>("StationIdStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UniteIdUnite")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdProduitConsommable");

                    b.HasIndex("StationIdStation");

                    b.HasIndex("UniteIdUnite");

                    b.ToTable("ProduitConsommable");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ProduitConsommable");
                });

            modelBuilder.Entity("Domain.Models.Role", b =>
                {
                    b.Property<Guid>("IdRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NomRole")
                        .HasColumnType("int");

                    b.HasKey("IdRole");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Domain.Models.SourceEau", b =>
                {
                    b.Property<Guid>("IdSourceEau")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descriminant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FkFiliale")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("VolumeEau")
                        .HasColumnType("float");

                    b.HasKey("IdSourceEau");

                    b.HasIndex("FkFiliale");

                    b.ToTable("SourceEau");

                    b.HasDiscriminator<string>("Descriminant").HasValue("SourceEau");
                });

            modelBuilder.Entity("Domain.Models.SourceEauEntretien", b =>
                {
                    b.Property<Guid>("IdSourceEauEntretien")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ChargeSourceEauEntretien")
                        .HasColumnType("float");

                    b.Property<string>("CodeSourceEauEntretien")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descriminant")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionSourceEauEntretien")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FkFournisseur")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkSourceEau")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsExternalSourceEauEntretien")
                        .HasColumnType("bit");

                    b.HasKey("IdSourceEauEntretien");

                    b.HasIndex("FkFournisseur");

                    b.HasIndex("FkSourceEau");

                    b.ToTable("SourceEauEntretien");
                });

            modelBuilder.Entity("Domain.Models.Station", b =>
                {
                    b.Property<Guid>("IdStation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CapaciteStation")
                        .HasColumnType("int");

                    b.Property<Guid?>("FkAtelier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActif")
                        .HasColumnType("bit");

                    b.Property<string>("NomStation")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("TypeAmmortissement")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdStation");

                    b.HasIndex("FkAtelier");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("Domain.Models.StationEntretien", b =>
                {
                    b.Property<Guid>("IdStationEntretien")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ChargeStationEntretien")
                        .HasColumnType("float");

                    b.Property<string>("DescriptionStationEntretien")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FkFournisseur")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsExternalStationEntretien")
                        .HasColumnType("bit");

                    b.Property<string>("NomStationEntretien")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("StationIdStation")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdStationEntretien");

                    b.HasIndex("FkFournisseur");

                    b.HasIndex("StationIdStation");

                    b.ToTable("StationEntretien");
                });

            modelBuilder.Entity("Domain.Models.StationParametre", b =>
                {
                    b.Property<Guid>("IdStationParametre")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkSuiviParametre")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StationIdStation")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SuiviParametreIdSuiviParametre")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdStationParametre");

                    b.HasIndex("StationIdStation");

                    b.HasIndex("SuiviParametreIdSuiviParametre");

                    b.ToTable("StationParametre");
                });

            modelBuilder.Entity("Domain.Models.SuiviParametre", b =>
                {
                    b.Property<Guid>("IdSuiviParametre")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FkSuiviType")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelSuiviParametre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SuiviTypeIdSuiviType")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdSuiviParametre");

                    b.HasIndex("SuiviTypeIdSuiviType");

                    b.ToTable("SuiviParametre");
                });

            modelBuilder.Entity("Domain.Models.SuiviQuotidien", b =>
                {
                    b.Property<Guid>("IdSuiviQuotidien")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateSuiviQuotidien")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FkStationParametre")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StationParametreIdStationParametre")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ValueSuiviQuotidien")
                        .HasColumnType("float");

                    b.HasKey("IdSuiviQuotidien");

                    b.HasIndex("StationParametreIdStationParametre");

                    b.ToTable("SuiviQuotidien");
                });

            modelBuilder.Entity("Domain.Models.SuiviType", b =>
                {
                    b.Property<Guid>("IdSuiviType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelSuiviType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdSuiviType");

                    b.ToTable("SuiviType");
                });

            modelBuilder.Entity("Domain.Models.TypeEquipment", b =>
                {
                    b.Property<Guid>("IdTypeEquipment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelTypeEquipment")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdTypeEquipment");

                    b.ToTable("TypeEquipment");
                });

            modelBuilder.Entity("Domain.Models.Unite", b =>
                {
                    b.Property<Guid>("IdUnite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelUnite")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUnite");

                    b.ToTable("Unite");
                });

            modelBuilder.Entity("Domain.Models.DosageChimique", b =>
                {
                    b.HasBaseType("Domain.Models.ProduitConsommable");

                    b.Property<double>("ConcentrationDosageChimique")
                        .HasColumnType("float");

                    b.Property<string>("ConfigurationPompe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FkProduitChimique")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProduitChimiqueIdProduitChimique")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ProduitChimiqueIdProduitChimique");

                    b.HasDiscriminator().HasValue("DosageChimique");
                });

            modelBuilder.Entity("Domain.Models.Bassin", b =>
                {
                    b.HasBaseType("Domain.Models.SourceEau");

                    b.Property<string>("NomBassin")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasDiscriminator().HasValue("Bassin");
                });

            modelBuilder.Entity("Domain.Models.Puit", b =>
                {
                    b.HasBaseType("Domain.Models.SourceEau");

                    b.Property<string>("NomPuit")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<double>("Profondeur")
                        .HasColumnType("float");

                    b.Property<string>("TypeAmortissement")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Puit");
                });

            modelBuilder.Entity("Domain.Models.Atelier", b =>
                {
                    b.HasOne("Domain.Models.Filiale", "Filiale")
                        .WithMany("Ateliers")
                        .HasForeignKey("FkFiliale")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Domain.Models.Compte", b =>
                {
                    b.HasOne("Domain.Models.Filiale", "Filiale")
                        .WithMany("Comptes")
                        .HasForeignKey("FkFiliale")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Domain.Models.Role", "Role")
                        .WithMany("Comptes")
                        .HasForeignKey("FkRole")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Domain.Models.Equipment", b =>
                {
                    b.HasOne("Domain.Models.NatureEquipment", "NatureEquipment")
                        .WithMany("Equipments")
                        .HasForeignKey("NatureEquipmentIdNatureEquipment");

                    b.HasOne("Domain.Models.Station", "Station")
                        .WithMany("Equipments")
                        .HasForeignKey("StationIdStation");

                    b.HasOne("Domain.Models.TypeEquipment", "TypeEquipment")
                        .WithMany("Equipments")
                        .HasForeignKey("TypeEquipmentIdTypeEquipment");
                });

            modelBuilder.Entity("Domain.Models.LavageChimique", b =>
                {
                    b.HasOne("Domain.Models.ProduitChimique", "ProduitChimique")
                        .WithMany("LavageChimiques")
                        .HasForeignKey("ProduitChimiqueIdProduitChimique");

                    b.HasOne("Domain.Models.Station", "Station")
                        .WithMany("LavagesChimiques")
                        .HasForeignKey("StationIdStation");
                });

            modelBuilder.Entity("Domain.Models.Objectif", b =>
                {
                    b.HasOne("Domain.Models.Station", "Station")
                        .WithMany("Objectifs")
                        .HasForeignKey("StationIdStation");
                });

            modelBuilder.Entity("Domain.Models.ProduitChimique", b =>
                {
                    b.HasOne("Domain.Models.CategorieProduitChimique", "CategorieProduitChimique")
                        .WithMany("produitsChimiques")
                        .HasForeignKey("CategorieProduitChimiqueIdCategorie");
                });

            modelBuilder.Entity("Domain.Models.ProduitConsommable", b =>
                {
                    b.HasOne("Domain.Models.Station", "Station")
                        .WithMany("ProduitsConsommables")
                        .HasForeignKey("StationIdStation");

                    b.HasOne("Domain.Models.Unite", "Unite")
                        .WithMany("ProduitsConsommables")
                        .HasForeignKey("UniteIdUnite");
                });

            modelBuilder.Entity("Domain.Models.SourceEau", b =>
                {
                    b.HasOne("Domain.Models.Filiale", "Filiale")
                        .WithMany("SourcesEaux")
                        .HasForeignKey("FkFiliale")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Domain.Models.SourceEauEntretien", b =>
                {
                    b.HasOne("Domain.Models.Fournisseur", "Fournisseur")
                        .WithMany("SourceEauEntretiens")
                        .HasForeignKey("FkFournisseur")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Domain.Models.SourceEau", "SourceEau")
                        .WithMany("SourceEauEntretiens")
                        .HasForeignKey("FkSourceEau")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Domain.Models.Station", b =>
                {
                    b.HasOne("Domain.Models.Atelier", "Atelier")
                        .WithMany("Stations")
                        .HasForeignKey("FkAtelier")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Domain.Models.StationEntretien", b =>
                {
                    b.HasOne("Domain.Models.Fournisseur", "Fournisseur")
                        .WithMany("StationEntretiens")
                        .HasForeignKey("FkFournisseur")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Models.Station", "Station")
                        .WithMany("StationEntretiens")
                        .HasForeignKey("StationIdStation");
                });

            modelBuilder.Entity("Domain.Models.StationParametre", b =>
                {
                    b.HasOne("Domain.Models.Station", "Station")
                        .WithMany("StationParametres")
                        .HasForeignKey("StationIdStation");

                    b.HasOne("Domain.Models.SuiviParametre", "SuiviParametre")
                        .WithMany("StationParametres")
                        .HasForeignKey("SuiviParametreIdSuiviParametre");
                });

            modelBuilder.Entity("Domain.Models.SuiviParametre", b =>
                {
                    b.HasOne("Domain.Models.SuiviType", "SuiviType")
                        .WithMany("SuiviParametres")
                        .HasForeignKey("SuiviTypeIdSuiviType");
                });

            modelBuilder.Entity("Domain.Models.SuiviQuotidien", b =>
                {
                    b.HasOne("Domain.Models.StationParametre", "StationParametre")
                        .WithMany("SuivisQuotidiens")
                        .HasForeignKey("StationParametreIdStationParametre");
                });

            modelBuilder.Entity("Domain.Models.DosageChimique", b =>
                {
                    b.HasOne("Domain.Models.ProduitChimique", "ProduitChimique")
                        .WithMany("DosageChimiques")
                        .HasForeignKey("ProduitChimiqueIdProduitChimique");
                });
#pragma warning restore 612, 618
        }
    }
}
