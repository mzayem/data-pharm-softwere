namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ProductModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                {
                    ProductID = c.Int(nullable: false, identity: false),
                    PackingType = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 150),
                    ProductCode = c.Long(nullable: false),
                    HSCode = c.Int(nullable: false),
                    PackingSize = c.String(nullable: false, maxLength: 250),
                    CartonSize = c.Int(nullable: false),
                    Uom = c.String(nullable: false, maxLength: 100),
                    PurchaseDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ReqGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    UnReqGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsAdvTaxExempted = c.Boolean(nullable: false),
                    IsGSTExempted = c.Boolean(nullable: false),
                    SubGroupID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.SubGroups", t => t.SubGroupID, cascadeDelete: true)
                .Index(t => t.SubGroupID);
            //Sql(@"
            //    DECLARE @currentId BIGINT = IDENT_CURRENT('Products');

            //    IF @currentId IS NULL OR @currentId < 101001
            //    BEGIN
            //        DBCC CHECKIDENT ('Products', RESEED, 101001);
            //    END
            //");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups");
            DropIndex("dbo.Products", new[] { "SubGroupID" });
            DropTable("dbo.Products");
        }
    }
}