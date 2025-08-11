namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPurchaseAndDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseDetail",
                c => new
                {
                    PurchaseDetailId = c.Int(nullable: false, identity: true),
                    PurchaseId = c.Int(nullable: false),
                    BatchId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    Batch_BatchID = c.Int(),
                })
                .PrimaryKey(t => t.PurchaseDetailId)
                .ForeignKey("dbo.Batches", t => t.BatchId)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .ForeignKey("dbo.Batches", t => t.Batch_BatchID)
                .Index(t => t.PurchaseId)
                .Index(t => t.BatchId)
                .Index(t => t.Batch_BatchID);

            CreateTable(
                "dbo.Purchases",
                c => new
                {
                    PurchaseId = c.Int(nullable: false, identity: true),
                    Posted = c.Boolean(nullable: false),
                    PurchaseDate = c.DateTime(nullable: false),
                    AdvTaxOn = c.Int(nullable: false),
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
                    Created_by = c.String(nullable: false, maxLength: 50),
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
            DropForeignKey("dbo.PurchaseDetail", "Batch_BatchID", "dbo.Batches");
            DropForeignKey("dbo.Purchases", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.PurchaseDetail", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseDetail", "BatchId", "dbo.Batches");
            DropIndex("dbo.Purchases", new[] { "VendorId" });
            DropIndex("dbo.PurchaseDetail", new[] { "Batch_BatchID" });
            DropIndex("dbo.PurchaseDetail", new[] { "BatchId" });
            DropIndex("dbo.PurchaseDetail", new[] { "PurchaseId" });
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseDetail");
        }
    }
}