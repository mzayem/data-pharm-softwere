using data_pharm_softwere.Models;
using System.Data.Entity;
using DataModel = data_pharm_softwere.Models.Data;

namespace data_pharm_softwere.Data
{
    public class DataPharmaContext : DbContext
    {
        public DataPharmaContext() : base("name=DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public DbSet<DataModel> Data { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBonus> ProductBonuses { get; set; }
        public DbSet<BatchStock> BatchesStock { get; set; }
        public DbSet<CityRoute> CityRoutes { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Salesman> Salesmen { get; set; }
        public DbSet<SalesmanTown> SalesmanTowns { get; set; }
        public DbSet<MedicalRep> MedicalReps { get; set; }
        public DbSet<MedicalRepSubGroup> MedicalRepSubGroups { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<DiscountPolicy> DiscountPolicies { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesDetail> SalesDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vendor>().ToTable("Vendors");
            modelBuilder.Entity<Division>().ToTable("Divisions");
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<SubGroup>().ToTable("SubGroups");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<BatchStock>().ToTable("BatchesStock");
            modelBuilder.Entity<CityRoute>().ToTable("CityRoutes");
            modelBuilder.Entity<Town>().ToTable("Towns");
            modelBuilder.Entity<Customer>().ToTable("Customers");

            // Vendor → Account
            modelBuilder.Entity<Account>()
                .HasOptional(a => a.Vendor)
                .WithRequired(v => v.Account)
                .WillCascadeOnDelete(true);

            // Division → Vendor
            modelBuilder.Entity<Division>()
                .HasRequired(d => d.Vendor)
                .WithMany(v => v.Divisions)
                .HasForeignKey(d => d.AccountId)
                .WillCascadeOnDelete(true);

            // Group → Division
            modelBuilder.Entity<Group>()
                .HasRequired(g => g.Division)
                .WithMany(v => v.Groups)
                .HasForeignKey(g => g.DivisionID)
                .WillCascadeOnDelete(true);

            // SubGroup → Group
            modelBuilder.Entity<SubGroup>()
                .HasRequired(sg => sg.Group)
                .WithMany(g => g.SubGroups)
                .HasForeignKey(sg => sg.GroupID)
                .WillCascadeOnDelete(true);

            // Product → SubGroup
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.SubGroup)
                .WithMany(sg => sg.Products)
                .HasForeignKey(p => p.SubGroupID)
                .WillCascadeOnDelete(true);

            // Product → Division
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Division)
                .WithMany(d => d.Products)
                .HasForeignKey(p => p.DivisionID)
                .WillCascadeOnDelete(false);

            // BatchStock → Product (optional)
            modelBuilder.Entity<BatchStock>()
                .HasRequired(b => b.Product)
                .WithMany(p => p.BatchesStock)
                .HasForeignKey(b => b.ProductID)
                .WillCascadeOnDelete(true);

            // Product → Bonus
            modelBuilder.Entity<ProductBonus>()
                .HasRequired(pb => pb.Product)
                .WithMany(p => p.ProductBonuses)
                .HasForeignKey(pb => pb.ProductID)
                .WillCascadeOnDelete(true);

            //Town → CityRoute
            modelBuilder.Entity<Town>()
                .HasRequired(t => t.CityRoute)
                .WithMany(cr => cr.Towns)
                .HasForeignKey(t => t.CityRouteID)
                .WillCascadeOnDelete(true);

            // Customer → Town
            modelBuilder.Entity<Customer>()
                .HasRequired(c => c.Town)
                .WithMany(t => t.Customers)
                .HasForeignKey(c => c.TownID)
                .WillCascadeOnDelete(true);

            //  SalesmanTown → Salesman
            modelBuilder.Entity<SalesmanTown>()
                .HasRequired(st => st.Salesman)
                .WithMany(s => s.SalesmanTowns)
                .HasForeignKey(st => st.SalesmanID)
                .WillCascadeOnDelete(true);

            // SalesmanTown → Town
            modelBuilder.Entity<SalesmanTown>()
                .HasRequired(st => st.Town)
                .WithMany(t => t.SalesmanTowns)
                .HasForeignKey(st => st.TownID)
                .WillCascadeOnDelete(true);

            // MedicalRepSubGroup → MedicalRep
            modelBuilder.Entity<MedicalRepSubGroup>()
                .HasRequired(ms => ms.MedicalRep)
                .WithMany(m => m.MedicalRepSubGroups)
                .HasForeignKey(ms => ms.MedicalRepID)
                .WillCascadeOnDelete(true);

            // MedicalRepSubGroup → SubGroup
            modelBuilder.Entity<MedicalRepSubGroup>()
                .HasRequired(ms => ms.SubGroup)
                .WithMany(sg => sg.MedicalRepSubGroups)
                .HasForeignKey(ms => ms.SubGroupID)
                .WillCascadeOnDelete(true);

            // Purchase → Vendor
            modelBuilder.Entity<Purchase>()
                .HasRequired(p => p.Vendor)
                .WithMany(v => v.purchases)
                .HasForeignKey(p => p.VendorId)
                .WillCascadeOnDelete(false);

            // Purchase → PurchaseDetails
            modelBuilder.Entity<Purchase>()
                .HasMany(p => p.PurchaseDetails)
                .WithRequired(pd => pd.Purchase)
                .HasForeignKey(pd => pd.PurchaseId)
                .WillCascadeOnDelete(true);

            // PurchaseDetail → BatchStock
            modelBuilder.Entity<PurchaseDetail>()
                .HasRequired(pd => pd.BatchStock)
                .WithMany()
                .HasForeignKey(pd => pd.BatchStockID)
                .WillCascadeOnDelete(false);

            // Customer → Account
            modelBuilder.Entity<Account>()
                .HasOptional(a => a.Customer)
                .WithRequired(c => c.Account)
                .WillCascadeOnDelete(true);

            // DiscountPolicy → Customer
            modelBuilder.Entity<DiscountPolicy>()
                .HasRequired(dp => dp.Customer)
                .WithMany(c => c.DiscountPolicies)
                .HasForeignKey(dp => dp.CustomerAccountId)
                .WillCascadeOnDelete(true);

            // DiscountPolicy → Product
            modelBuilder.Entity<DiscountPolicy>()
                .HasRequired(dp => dp.Product)
                .WithMany(p => p.DiscountPolicies)
                .HasForeignKey(dp => dp.ProductID)
                .WillCascadeOnDelete(false);

            // Sales → Customer
            modelBuilder.Entity<Sales>()
                .HasRequired(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .WillCascadeOnDelete(false);

            // Sales → SalesDetails
            modelBuilder.Entity<Sales>()
                .HasMany(s => s.SalesDetails)
                .WithRequired(sd => sd.Sales)
                .HasForeignKey(sd => sd.SalesId)
                .WillCascadeOnDelete(true);

            // SalesDetail → BatchStock
            modelBuilder.Entity<SalesDetail>()
                .HasRequired(sd => sd.BatchStock)
                .WithMany()
                .HasForeignKey(sd => sd.BatchStockID)
                .WillCascadeOnDelete(false);

            // ================= Sales → SalesmanTown Relations =================
            modelBuilder.Entity<Sales>()
                .HasRequired(s => s.SalesmanBooker)
                .WithMany()
                .HasForeignKey(s => s.SalesmanBookerTownId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sales>()
                .HasRequired(s => s.SalesmanSupplier)
                .WithMany()
                .HasForeignKey(s => s.SalesmanSupplierTownId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sales>()
                .HasRequired(s => s.SalesmanDriver)
                .WithMany()
                .HasForeignKey(s => s.SalesmanDriverTownId)
                .WillCascadeOnDelete(false);
        }
    }
}