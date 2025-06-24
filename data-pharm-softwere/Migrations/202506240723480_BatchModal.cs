namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BatchModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Batches",
                c => new
                    {
                        BatchID = c.Int(nullable: false, identity: true),
                        BatchNo = c.Int(nullable: false),
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
                        ProductID = c.Int(),
                    })
                .PrimaryKey(t => t.BatchID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Batches", "ProductID", "dbo.Products");
            DropIndex("dbo.Batches", new[] { "ProductID" });
            DropTable("dbo.Batches");
        }
    }
}
