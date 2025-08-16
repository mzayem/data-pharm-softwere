namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BatchStock : DbMigration
    {
        public override void Up()
        {
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
        }

        public override void Down()
        {
            DropForeignKey("dbo.BatchesStock", "ProductID", "dbo.Products");
            DropIndex("dbo.BatchesStock", new[] { "ProductID" });
            DropTable("dbo.BatchesStock");
        }
    }
}