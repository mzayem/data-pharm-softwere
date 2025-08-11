namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BatchStock : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Batches", "ProductID", "dbo.Products");
            DropForeignKey("dbo.PurchaseDetail", "BatchID", "dbo.Batches");
            DropForeignKey("dbo.PurchaseDetail", "Batch_BatchID", "dbo.Batches");
            DropIndex("dbo.Batches", new[] { "ProductID" });
            DropIndex("dbo.PurchaseDetail", new[] { "BatchID" });
            DropIndex("dbo.PurchaseDetail", new[] { "Batch_BatchID" });
            CreateTable(
                "dbo.BatchesStock",
                c => new
                {
                    BatchStockID = c.Int(nullable: false, identity: true),
                    BatchNo = c.String(nullable: false, maxLength: 50),
                    MFGDate = c.DateTime(nullable: false),
                    ExpiryDate = c.DateTime(nullable: false),
                    DP = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TP = c.Decimal(nullable: false, precision: 18, scale: 2),
                    MRP = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CartonUnits = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CartonDp = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AvailableQty = c.Int(nullable: false),
                    InTransitQty = c.Int(nullable: false),
                    UnavailableQty = c.Int(nullable: false),
                    CreatedBy = c.String(nullable: false, maxLength: 50),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedBy = c.String(maxLength: 50),
                    UpdatedAt = c.DateTime(),
                    ProductID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.BatchStockID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);

            AddColumn("dbo.PurchaseDetail", "BatchStockID", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseDetail", "BatchStock_BatchStockID", c => c.Int());
            CreateIndex("dbo.PurchaseDetail", "BatchStockID");
            CreateIndex("dbo.PurchaseDetail", "BatchStock_BatchStockID");
            AddForeignKey("dbo.PurchaseDetail", "BatchStockID", "dbo.BatchesStock", "BatchStockID");
            AddForeignKey("dbo.PurchaseDetail", "BatchStock_BatchStockID", "dbo.BatchesStock", "BatchStockID");
            DropColumn("dbo.PurchaseDetail", "BatchID");
            DropColumn("dbo.PurchaseDetail", "Batch_BatchID");
            DropTable("dbo.Batches");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Batches",
                c => new
                {
                    BatchID = c.Int(nullable: false, identity: true),
                    BatchNo = c.String(nullable: false, maxLength: 50),
                    MFGDate = c.DateTime(nullable: false),
                    ExpiryDate = c.DateTime(nullable: false),
                    DP = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TP = c.Decimal(nullable: false, precision: 18, scale: 2),
                    MRP = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CartonQty = c.Int(nullable: false),
                    CartonPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CreatedBy = c.String(nullable: false, maxLength: 50),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedBy = c.String(maxLength: 50),
                    UpdatedAt = c.DateTime(),
                    ProductID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.BatchID);

            AddColumn("dbo.PurchaseDetail", "Batch_BatchID", c => c.Int());
            AddColumn("dbo.PurchaseDetail", "BatchID", c => c.Int(nullable: false));
            DropForeignKey("dbo.PurchaseDetail", "BatchStock_BatchStockID", "dbo.BatchesStock");
            DropForeignKey("dbo.PurchaseDetail", "BatchStockID", "dbo.BatchesStock");
            DropForeignKey("dbo.BatchesStock", "ProductID", "dbo.Products");
            DropIndex("dbo.PurchaseDetail", new[] { "BatchStock_BatchStockID" });
            DropIndex("dbo.PurchaseDetail", new[] { "BatchStockID" });
            DropIndex("dbo.BatchesStock", new[] { "ProductID" });
            DropColumn("dbo.PurchaseDetail", "BatchStock_BatchStockID");
            DropColumn("dbo.PurchaseDetail", "BatchStockID");
            DropTable("dbo.BatchesStock");
            CreateIndex("dbo.PurchaseDetail", "Batch_BatchID");
            CreateIndex("dbo.PurchaseDetail", "BatchID");
            CreateIndex("dbo.Batches", "ProductID");
            AddForeignKey("dbo.PurchaseDetail", "Batch_BatchID", "dbo.Batches", "BatchID");
            AddForeignKey("dbo.PurchaseDetail", "BatchID", "dbo.Batches", "BatchID");
            AddForeignKey("dbo.Batches", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
        }
    }
}