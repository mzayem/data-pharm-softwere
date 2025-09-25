namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SalesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sales",
                c => new
                {
                    SalesId = c.Int(nullable: false, identity: true),
                    VoucherNumber = c.String(nullable: false, maxLength: 20),
                    VoucherType = c.Int(nullable: false),
                    Posted = c.Boolean(nullable: false),
                    SalesDate = c.DateTime(nullable: false),
                    AdvTaxOn = c.Int(nullable: false),
                    GSTType = c.Int(nullable: false),
                    BillType = c.Int(nullable: false),
                    Remarks = c.String(nullable: true, maxLength: 200),
                    AdvTaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AdvTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AdditionalCharges = c.Decimal(nullable: false, precision: 18, scale: 2),
                    DiscountedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    GrossAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CustomerId = c.Int(nullable: false),
                    SalesmanBookerTownId = c.Int(nullable: false),
                    SalesmanSupplierTownId = c.Int(nullable: false),
                    SalesmanDriverTownId = c.Int(nullable: false),
                    CreatedBy = c.String(nullable: false, maxLength: 50),
                    CreatedAt = c.DateTime(nullable: false),
                    Updatedby = c.String(maxLength: 50),
                    UpdatedAt = c.DateTime(),
                    SalesmanTown_SalesmanTownID = c.Int(),
                    SalesmanTown_SalesmanTownID1 = c.Int(),
                    SalesmanTown_SalesmanTownID2 = c.Int(),
                })
                .PrimaryKey(t => t.SalesId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.SalesmanTowns", t => t.SalesmanTown_SalesmanTownID)
                .ForeignKey("dbo.SalesmanTowns", t => t.SalesmanTown_SalesmanTownID1)
                .ForeignKey("dbo.SalesmanTowns", t => t.SalesmanTown_SalesmanTownID2)
                .ForeignKey("dbo.SalesmanTowns", t => t.SalesmanBookerTownId)
                .ForeignKey("dbo.SalesmanTowns", t => t.SalesmanDriverTownId)
                .ForeignKey("dbo.SalesmanTowns", t => t.SalesmanSupplierTownId)
                .Index(t => t.CustomerId)
                .Index(t => t.SalesmanBookerTownId)
                .Index(t => t.SalesmanSupplierTownId)
                .Index(t => t.SalesmanDriverTownId)
                .Index(t => t.SalesmanTown_SalesmanTownID)
                .Index(t => t.SalesmanTown_SalesmanTownID1)
                .Index(t => t.SalesmanTown_SalesmanTownID2);

            CreateTable(
                "dbo.SalesDetail",
                c => new
                {
                    SalesDetailId = c.Int(nullable: false, identity: true),
                    SalesId = c.Int(nullable: false),
                    BatchStockID = c.Int(nullable: false),
                    Qty = c.Int(nullable: false),
                    BonusQty = c.Int(nullable: false),
                    DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    GSTAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.SalesDetailId)
                .ForeignKey("dbo.BatchesStock", t => t.BatchStockID)
                .ForeignKey("dbo.Sales", t => t.SalesId, cascadeDelete: true)
                .Index(t => t.SalesId)
                .Index(t => t.BatchStockID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Sales", "SalesmanSupplierTownId", "dbo.SalesmanTowns");
            DropForeignKey("dbo.Sales", "SalesmanDriverTownId", "dbo.SalesmanTowns");
            DropForeignKey("dbo.Sales", "SalesmanBookerTownId", "dbo.SalesmanTowns");
            DropForeignKey("dbo.Sales", "SalesmanTown_SalesmanTownID2", "dbo.SalesmanTowns");
            DropForeignKey("dbo.Sales", "SalesmanTown_SalesmanTownID1", "dbo.SalesmanTowns");
            DropForeignKey("dbo.Sales", "SalesmanTown_SalesmanTownID", "dbo.SalesmanTowns");
            DropForeignKey("dbo.SalesDetail", "SalesId", "dbo.Sales");
            DropForeignKey("dbo.SalesDetail", "BatchStockID", "dbo.BatchesStock");
            DropForeignKey("dbo.Sales", "CustomerId", "dbo.Customers");
            DropIndex("dbo.SalesDetail", new[] { "BatchStockID" });
            DropIndex("dbo.SalesDetail", new[] { "SalesId" });
            DropIndex("dbo.Sales", new[] { "SalesmanTown_SalesmanTownID2" });
            DropIndex("dbo.Sales", new[] { "SalesmanTown_SalesmanTownID1" });
            DropIndex("dbo.Sales", new[] { "SalesmanTown_SalesmanTownID" });
            DropIndex("dbo.Sales", new[] { "SalesmanDriverTownId" });
            DropIndex("dbo.Sales", new[] { "SalesmanSupplierTownId" });
            DropIndex("dbo.Sales", new[] { "SalesmanBookerTownId" });
            DropIndex("dbo.Sales", new[] { "CustomerId" });
            DropTable("dbo.SalesDetail");
            DropTable("dbo.Sales");
        }
    }
}