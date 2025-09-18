namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TableUpadtes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Purchases", "VendorId", "dbo.Vendors");
            AddColumn("dbo.DiscountPolicies", "CreditDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DiscountPolicies", "CreditDiscountExpiry", c => c.DateTime());
            AddColumn("dbo.DiscountPolicies", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.DiscountPolicies", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.PurchaseDetail", "Qty", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseDetail", "BonusQty", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseDetail", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PurchaseDetail", "GSTAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SalesmanTowns", "AssignmentType", c => c.Int(nullable: false));
            AddColumn("dbo.SalesmanTowns", "Percentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddForeignKey("dbo.Purchases", "VendorId", "dbo.Vendors", "AccountId");
            DropColumn("dbo.DiscountPolicies", "SpecialDiscount");
            DropColumn("dbo.DiscountPolicies", "SpecialDiscountExpiry");
        }

        public override void Down()
        {
            AddColumn("dbo.DiscountPolicies", "SpecialDiscountExpiry", c => c.DateTime());
            AddColumn("dbo.DiscountPolicies", "SpecialDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.Purchases", "VendorId", "dbo.Vendors");
            DropColumn("dbo.SalesmanTowns", "Percentage");
            DropColumn("dbo.SalesmanTowns", "AssignmentType");
            DropColumn("dbo.PurchaseDetail", "GSTAmount");
            DropColumn("dbo.PurchaseDetail", "DiscountAmount");
            DropColumn("dbo.PurchaseDetail", "BonusQty");
            DropColumn("dbo.PurchaseDetail", "Qty");
            DropColumn("dbo.DiscountPolicies", "UpdatedAt");
            DropColumn("dbo.DiscountPolicies", "CreatedAt");
            DropColumn("dbo.DiscountPolicies", "CreditDiscountExpiry");
            DropColumn("dbo.DiscountPolicies", "CreditDiscount");
            AddForeignKey("dbo.Purchases", "VendorId", "dbo.Vendors", "AccountId", cascadeDelete: true);
        }
    }
}