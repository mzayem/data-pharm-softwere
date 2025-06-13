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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vendor>().ToTable("Vendors");
        }
    }
}