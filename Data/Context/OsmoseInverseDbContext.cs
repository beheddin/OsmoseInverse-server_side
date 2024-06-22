using System;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Context
{
    public class OsmoseInverseDbContext : DbContext
    {
        public OsmoseInverseDbContext(DbContextOptions<OsmoseInverseDbContext> options) : base(options) { }

        // entities DbSets
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Filiale> Filiales { get; set; }
        public DbSet<Atelier> Ateliers { get; set; }

        public DbSet<Station> Stations { get; set; }
        public DbSet<StationEntretien> StationEntretiens { get; set; }

        //public DbSet<SourceEau> SourceEaux{ get; set; }
        public DbSet<Bassin> Bassins { get; set; }
        public DbSet<Puit> Puits { get; set; }

        public DbSet<SourceEauEntretien> SourceEauEntretiens { get; set; }
        //public DbSet<PuitEntretien> PuitEntretiens { get; set; }
        //public DbSet<BassinEntretien> BassinEntretiens { get; set; }

        public DbSet<Fournisseur> Fournisseurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();    //enable sensitive data logging during development
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            //rename entities tables from plural to singular:
            // Get all the entities in the DbContext
            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();

            foreach (var entityType in entityTypes)
            {
                // Get the current table name
                var tableName = entityType.GetTableName();

                // Make the table name singular if it's plural ending with "s" (but not "ss")
                if (tableName.EndsWith("s") && !tableName.EndsWith("ss"))
                {
                    var singularTableName = tableName.Substring(0, tableName.Length - 1);
                    entityType.SetTableName(singularTableName);
                }

                // Make the table name singular if it's plural ending with "x"
                else if (tableName.EndsWith("x"))
                {
                    var singularTableName = tableName.Substring(0, tableName.Length - 1);
                    entityType.SetTableName(singularTableName);
                }
            }

            ////map the enum to a string in the database for 'Descriminant' of type 'TypeSourceEau' in 'SourceEau' entity, using the' ValueConverter' feature in EF Core
            //var converter = new ValueConverter<TypeSourceEau, string>(
            //v => v.ToString(),
            //v => (TypeSourceEau)Enum.Parse(typeof(TypeSourceEau), v));

            //modelBuilder.Entity<SourceEau>(entity =>
            //{
            //    entity.Property(e => e.Descriminant)
            //          .HasConversion(converter)
            //          .HasMaxLength(10);
            //});

            //NomCompte  is unique
            //modelBuilder.Entity<Compte>(entity => { entity.HasIndex(e=>e.NomCompte).IsUnique(); }) ;

            #region Primary keys
            modelBuilder.Entity<Compte>().HasKey(compte => compte.IdCompte);
            modelBuilder.Entity<Role>().HasKey(role => role.IdRole);

            modelBuilder.Entity<Filiale>().HasKey(filiale => filiale.IdFiliale);
            modelBuilder.Entity<Atelier>().HasKey(atelier => atelier.IdAtelier);

            modelBuilder.Entity<Station>().HasKey(station => station.IdStation);
            modelBuilder.Entity<StationEntretien>().HasKey(station => station.IdStationEntretien);

            modelBuilder.Entity<SourceEau>().HasKey(sourceEau => sourceEau.IdSourceEau);
            //modelBuilder.Entity<Puit>().HasKey(sourceEau => sourceEau.IdSourceEau);
            //modelBuilder.Entity<Bassin>().HasKey(sourceEau => sourceEau.IdSourceEau);
            modelBuilder.Entity<SourceEauEntretien>().HasKey(sourceEauEntretien => sourceEauEntretien.IdSourceEauEntretien);
            
            modelBuilder.Entity<Fournisseur>().HasKey(c => c.IdFournisseur);

            //modelBuilder.Entity<ProduitChimique>().HasKey(c => c.ProduitChimiqueId);
            //modelBuilder.Entity<CategorieProduitChimique>().HasKey(c => c.CategorieProduitChimiqueId);
            #endregion

            #region One to many relations
            modelBuilder.Entity<Compte>()
           .HasOne(compte => compte.Role)
           .WithMany(role => role.Comptes)
           .HasForeignKey(compte => compte.FkRole)
           .OnDelete(DeleteBehavior.SetNull);  // Set foreign key to null on delete
                                               //.OnDelete(DeleteBehavior.Restrict);  //prevent the deletion of a Role if there is at least 1 Compte associated with it
                                               //.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Compte>()
           .HasOne(compte => compte.Filiale)
           .WithMany(filiale => filiale.Comptes)
           .HasForeignKey(compte => compte.FkFiliale)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Station>()
           .HasOne(station => station.Atelier)
           .WithMany(atelier => atelier.Stations)
           .HasForeignKey(station => station.FkAtelier)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StationEntretien>()
           .HasOne(stationEntretien => stationEntretien.Station)
           .WithMany(station => station.StationEntretiens)
           .HasForeignKey(station => station.FkStation)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StationEntretien>()
           .HasOne(stationEntretien => stationEntretien.Fournisseur)
           .WithMany(fournisseur => fournisseur.StationEntretiens)
           .HasForeignKey(fournisseur => fournisseur.FkFournisseur)
           .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<SourceEau>()
            //.HasDiscriminator<string>("Descriminant")
            //.HasValue<Bassin>("Bassin")
            //.HasValue<Puit>("Puit");

            modelBuilder.Entity<SourceEau>()
            .HasDiscriminator<TypeSourceEau>("Descriminant")
            .HasValue<Bassin>(TypeSourceEau.Bassin)
            .HasValue<Puit>(TypeSourceEau.Puit);

            modelBuilder.Entity<SourceEau>()
           .HasOne(sourceEau => sourceEau.Filiale)
           .WithMany(filiale => filiale.SourcesEaux)
           .HasForeignKey(sourceEau => sourceEau.FkFiliale)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Puit>();
            modelBuilder.Entity<Bassin>();

            modelBuilder.Entity<SourceEauEntretien>()
           .HasOne(sourceEauEntretien => sourceEauEntretien.Fournisseur)
           .WithMany(fournisseur => fournisseur.SourceEauEntretiens)
           .HasForeignKey(sourceEauEntretien => sourceEauEntretien.FkFournisseur)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SourceEauEntretien>()
            .HasOne(sourceEauEntretien => sourceEauEntretien.SourceEau)
            .WithMany(sourceEau => sourceEau.SourceEauEntretiens)
            .HasForeignKey(sourceEauEntretien => sourceEauEntretien.FkSourceEau)
            .OnDelete(DeleteBehavior.SetNull);

           // modelBuilder.Entity<PuitEntretien>()
           //.HasOne(puitEntretien => puitEntretien.Puit)
           //.WithMany(puit => puit.PuitEntretiens)
           //.HasForeignKey(puitEntretien => puitEntretien.FkPuit)
           //.OnDelete(DeleteBehavior.SetNull);

           // modelBuilder.Entity<BassinEntretien>()
           //.HasOne(bassinEntretien => bassinEntretien.Bassin)
           //.WithMany(bassin => bassin.BassinEntretiens)
           //.HasForeignKey(bassinEntretien => bassinEntretien.FkBassin)
           //.OnDelete(DeleteBehavior.SetNull);
            
            #endregion

            #region Many to one relations
            modelBuilder.Entity<Role>()
                        .HasMany(role => role.Comptes)
                        .WithOne(compte => compte.Role)
                        .HasForeignKey(compte => compte.FkRole)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Filiale>()
                        .HasMany(filiale => filiale.Comptes)
                        .WithOne(compte => compte.Filiale)
                        .HasForeignKey(compte => compte.FkFiliale)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Filiale>()
                        .HasMany(filiale => filiale.Ateliers)
                        .WithOne(atelier => atelier.Filiale)
                        .HasForeignKey(atelier => atelier.FkFiliale)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Filiale>()
                        .HasMany(filiale => filiale.SourcesEaux)
                        .WithOne(sourceEau => sourceEau.Filiale)
                        .HasForeignKey(sourceEau => sourceEau.FkFiliale)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Station>()
                        .HasMany(station => station.StationEntretiens)
                        .WithOne(stationEntretiens => stationEntretiens.Station)
                        .HasForeignKey(stationEntretiens => stationEntretiens.FkStation)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Atelier>()
                        .HasMany(atelier => atelier.Stations)
                        .WithOne(station => station.Atelier)
                        .HasForeignKey(station => station.FkAtelier)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SourceEau>()
                        .HasMany(sourceEau => sourceEau.SourceEauEntretiens)
                        .WithOne(sourceEauEntretien => sourceEauEntretien.SourceEau)
                        .HasForeignKey(sourceEauEntretien => sourceEauEntretien.FkSourceEau)
                        .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<Puit>()
            //            .HasMany(puit => puit.PuitEntretiens)
            //            .WithOne(puitEntretien => puitEntretien.Puit)
            //            .HasForeignKey(puitEntretien => puitEntretien.FkPuit)
            //            .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<Bassin>()
            //            .HasMany(bassin => bassin.BassinEntretiens)
            //            .WithOne(bassinEntretien => bassinEntretien.Bassin)
            //            .HasForeignKey(bassinEntretien => bassinEntretien.FkBassin)
            //            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Fournisseur>()
            .HasMany(fournisseur => fournisseur.StationEntretiens)
            .WithOne(stationEntretien => stationEntretien.Fournisseur)
            .HasForeignKey(stationEntretien => stationEntretien.FkFournisseur)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Fournisseur>()
            .HasMany(fournisseur => fournisseur.SourceEauEntretiens)
            .WithOne(sourceEauEntretien => sourceEauEntretien.Fournisseur)
            .HasForeignKey(sourceEauEntretien => sourceEauEntretien.FkFournisseur)
            .OnDelete(DeleteBehavior.Cascade);


            //modelBuilder.Entity<CategorieProduitChimique>()
            //            .HasMany(o => o.ProduitChimique)
            //            .WithOne(w => w.CategorieProduitChimique)
            //            .HasForeignKey(fk => fk.FkCategorieProduitChimique)
            //            .OnDelete(DeleteBehavior.Cascade);

            #endregion

            base.OnModelCreating(modelBuilder);

            //#region Default values
            //// NomRole default value is Compte
            //modelBuilder.Entity<Role>()
            //.Property(r => r.NomRole)
            //.HasDefaultValue(TypeRole.Compte);
            //#endregion
        }
    }
}
