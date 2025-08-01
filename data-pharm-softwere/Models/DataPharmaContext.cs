﻿using data_pharm_softwere.Models;
using System.Data.Entity;

namespace data_pharm_softwere.Data
{
    public class DataPharmaContext : DbContext
    {
        public DataPharmaContext() : base("name=DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<CityRoute> CityRoutes { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Salesman> Salesmen { get; set; }
        public DbSet<SalesmanTown> SalesmanTowns { get; set; }
        public DbSet<MedicalRep> MedicalReps { get; set; }
        public DbSet<MedicalRepSubGroup> MedicalRepSubGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vendor>().ToTable("Vendors");
            modelBuilder.Entity<Division>().ToTable("Divisions");
            modelBuilder.Entity<Group>().ToTable("Groups");
            modelBuilder.Entity<SubGroup>().ToTable("SubGroups");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Batch>().ToTable("Batches");
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

            // Batch → Product (optional)
            modelBuilder.Entity<Batch>()
                .HasOptional(b => b.Product)
                .WithMany(p => p.Batches)
                .HasForeignKey(b => b.ProductID)
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
        }
    }
}