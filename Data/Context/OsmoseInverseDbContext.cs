using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Linq;


namespace Data.Context
{
    public class OsmoseInverseDbContext : DbContext
    {
        public OsmoseInverseDbContext(DbContextOptions<OsmoseInverseDbContext> options) : base(options) { }

        // entities DbSets
        public DbSet<User> Users { get; set; }
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

            //user's email is unique
            //modelBuilder.Entity<User>(entity => { entity.HasIndex(e=>e.Email).IsUnique(); }) ;

            #region Primary keys
            modelBuilder.Entity<User>().HasKey(user => user.UserId);
            modelBuilder.Entity<Role>().HasKey(role => role.RoleId);

            modelBuilder.Entity<Filiale>().HasKey(filiale => filiale.FilialeId);
            modelBuilder.Entity<Atelier>().HasKey(atelier => atelier.AtelierId);
            modelBuilder.Entity<Station>().HasKey(station => station.StationId);

            //modelBuilder.Entity<SourceEau>().HasKey(c => c.SourceEauId);
            //modelBuilder.Entity<ProduitChimique>().HasKey(c => c.ProduitChimiqueId);
            //modelBuilder.Entity<CategorieProduitChimique>().HasKey(c => c.CategorieProduitChimiqueId);
            //modelBuilder.Entity<Fournisseur>().HasKey(c => c.FournisseurId);
            #endregion

            #region One to many relations
            modelBuilder.Entity<User>()
            .HasOne(user => user.Role)
            .WithMany(role => role.Users)
            .HasForeignKey(user => user.FkRole)
            .OnDelete(DeleteBehavior.SetNull);  // Set foreign key to null on delete
                                                //.OnDelete(DeleteBehavior.Restrict);  //prevent the deletion of a Role if there is at least 1 User associated with it
                                                //.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
           .HasOne(user => user.Filiale)
           .WithMany(filiale => filiale.Users)
           .HasForeignKey(user => user.FkFiliale)
           .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Station>()
           .HasOne(station => station.Atelier)
           .WithMany(atelier => atelier.Stations)
           .HasForeignKey(station => station.FkAtelier)
           .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region Many to one relations
            modelBuilder.Entity<Role>()
                        .HasMany(role => role.Users)
                        .WithOne(user => user.Role)
                        .HasForeignKey(user => user.FkRole)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Filiale>()
                        .HasMany(filiale => filiale.Users)
                        .WithOne(user => user.Filiale)
                        .HasForeignKey(user => user.FkFiliale)
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
            //// RoleLabel default value is User
            //modelBuilder.Entity<Role>()
            //.Property(r => r.RoleLabel)
            //.HasDefaultValue(RoleType.User);
            //#endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
