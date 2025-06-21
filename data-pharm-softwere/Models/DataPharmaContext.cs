using System.Data.Entity;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Data
{
    public class DataPharmaContext : DbContext
    {
        public DataPharmaContext() : base("name=DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Batch> Batches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vendor>().ToTable("Vendors");
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<SubGroup>().ToTable("SubGroups");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Batch>().ToTable("Batches");

            modelBuilder.Entity<Group>()
                .HasRequired(g => g.Vendor)
                .WithMany()
                .HasForeignKey(g => g.VendorID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SubGroup>()
                .HasRequired(sg => sg.Group)
                .WithMany(g => g.SubGroups)
                .HasForeignKey(sg => sg.GroupID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Product>()
                .HasRequired(p => p.SubGroup)
                .WithMany()
                .HasForeignKey(p => p.SubGroupID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Batch>()
                .HasOptional(b => b.Product)
                .WithMany(p => p.Batches)
                .HasForeignKey(b => b.ProductID)
                .WillCascadeOnDelete(true);
        }
    }
}