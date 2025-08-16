namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseDetail",
                c => new
                    {
                        PurchaseDetailId = c.Int(nullable: false, identity: true),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseId = c.Int(nullable: false),
                        BatchStockID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        BatchStock_BatchStockID = c.Int(),
                    })
                .PrimaryKey(t => t.PurchaseDetailId)
                .ForeignKey("dbo.BatchesStock", t => t.BatchStockID)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .ForeignKey("dbo.BatchesStock", t => t.BatchStock_BatchStockID)
                .Index(t => t.PurchaseId)
                .Index(t => t.BatchStockID)
                .Index(t => t.BatchStock_BatchStockID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        Posted = c.Boolean(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        AdvTaxOn = c.Int(nullable: false),
                        GSTType = c.Int(nullable: false),
                        PoNumber = c.String(maxLength: 100),
                        Reference = c.String(maxLength: 200),
                        PurchaseType = c.Int(nullable: false),
                        AdvTaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdvTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdditionalCharges = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VendorId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                        Updatedby = c.String(maxLength: 50),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.VendorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseDetail", "BatchStock_BatchStockID", "dbo.BatchesStock");
            DropForeignKey("dbo.Purchases", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.PurchaseDetail", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseDetail", "BatchStockID", "dbo.BatchesStock");
            DropIndex("dbo.Purchases", new[] { "VendorId" });
            DropIndex("dbo.PurchaseDetail", new[] { "BatchStock_BatchStockID" });
            DropIndex("dbo.PurchaseDetail", new[] { "BatchStockID" });
            DropIndex("dbo.PurchaseDetail", new[] { "PurchaseId" });
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseDetail");
        }
    }
}
