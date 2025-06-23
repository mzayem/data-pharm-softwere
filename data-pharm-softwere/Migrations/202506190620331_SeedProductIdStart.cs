namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedProductIdStart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                {
                    ProductID = c.Int(nullable: false, identity: true),
                    ProductCode = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 150),
                    PackingType = c.Int(nullable: false),
                    Uom = c.String(nullable: false, maxLength: 100),
                    PackingSize = c.String(nullable: false, maxLength: 250),
                    CartonSize = c.String(nullable: false, maxLength: 100),
                    PurchaseDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Type = c.Int(nullable: false),
                    HSCode = c.Int(nullable: false),
                    ReqGST = c.Int(nullable: false),
                    UnReqGST = c.Int(nullable: false),
                    IsAdvTaxExempted = c.Boolean(nullable: false),
                    IsGSTExempted = c.Boolean(nullable: false),
                    VendorID = c.Int(nullable: false),
                    GroupID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Groups", t => t.GroupID)
                .ForeignKey("dbo.Vendors", t => t.VendorID)
                .Index(t => t.VendorID)
                .Index(t => t.GroupID);
            Sql(@"
                DECLARE @currentId BIGINT = IDENT_CURRENT('Products');

                IF @currentId IS NULL OR @currentId < 101001
                BEGIN
                    DBCC CHECKIDENT ('Products', RESEED, 101001);
                END
            ");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "VendorID", "dbo.Vendors");
            DropForeignKey("dbo.Products", "GroupID", "dbo.Groups");
            DropIndex("dbo.Products", new[] { "GroupID" });
            DropIndex("dbo.Products", new[] { "VendorID" });
            DropTable("dbo.Products");
        }
    }
}