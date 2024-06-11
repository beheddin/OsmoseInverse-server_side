using Microsoft.EntityFrameworkCore;
using Domain.Models;
using System.Linq;


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

        //public DbSet<SourceEau> SourceEaus { get; set; }
        //public DbSet<Puit> Puits { get; set; }
        //public DbSet<Bassin> Bassins { get; set; }

        //public DbSet<ProduitChimique> ProduitChimiques { get; set; }
        //public DbSet<CategorieProduitChimique> CategorieProduitChimiques { get; set; }

        //public DbSet<Fournisseur> Fournisseurs { get; set; }

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

                // Make the table name singular if it's plural
                if (tableName.EndsWith("s") && !tableName.EndsWith("ss"))
                {
                    var singularTableName = tableName.Substring(0, tableName.Length - 1);
                    entityType.SetTableName(singularTableName);
                }
            }

            //compte's email is unique
            //modelBuilder.Entity<Compte>(entity => { entity.HasIndex(e=>e.Email).IsUnique(); }) ;

            #region Primary keys
            modelBuilder.Entity<Compte>().HasKey(compte => compte.IdCompte);
            modelBuilder.Entity<Role>().HasKey(role => role.IdRole);

            modelBuilder.Entity<Filiale>().HasKey(filiale => filiale.IdFiliale);
            modelBuilder.Entity<Atelier>().HasKey(atelier => atelier.IdAtelier);
            modelBuilder.Entity<Station>().HasKey(station => station.IdStation);

            //modelBuilder.Entity<SourceEau>().HasKey(c => c.SourceEauId);
            //modelBuilder.Entity<ProduitChimique>().HasKey(c => c.ProduitChimiqueId);
            //modelBuilder.Entity<CategorieProduitChimique>().HasKey(c => c.CategorieProduitChimiqueId);
            //modelBuilder.Entity<Fournisseur>().HasKey(c => c.FournisseurId);
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

            modelBuilder.Entity<Atelier>()
                        .HasMany(atelier => atelier.Stations)
                        .WithOne(station => station.Atelier)
                        .HasForeignKey(station => station.FkAtelier)
                        .OnDelete(DeleteBehavior.SetNull);



            //modelBuilder.Entity<CategorieProduitChimique>()
            //            .HasMany(o => o.ProduitChimique)
            //            .WithOne(w => w.CategorieProduitChimique)
            //            .HasForeignKey(fk => fk.FkCategorieProduitChimique)
            //            .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Fournisseur>()
            //.HasMany(o => o.Station)
            //.WithOne(w => w.Fournisseur)
            //.HasForeignKey(fk => fk.FkFournisseur)
            //.OnDelete(DeleteBehavior.Cascade);
            #endregion

            //#region Default values
            //// NomRole default value is Compte
            //modelBuilder.Entity<Role>()
            //.Property(r => r.NomRole)
            //.HasDefaultValue(RoleType.Compte);
            //#endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
