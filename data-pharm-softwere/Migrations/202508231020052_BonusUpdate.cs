namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BonusUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductBonus",
                c => new
                {
                    ProductBonusID = c.Int(nullable: false, identity: true),
                    ProductID = c.Int(nullable: false),
                    MinQty = c.Int(nullable: false),
                    BonusItems = c.Int(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    AssignedOn = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ProductBonusID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);

            AddColumn("dbo.Products", "IsDiscounted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "BonusType", c => c.Int(nullable: false));
            AddColumn("dbo.BatchesStock", "BonusQty", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.ProductBonus", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductBonus", new[] { "ProductID" });
            DropColumn("dbo.BatchesStock", "BonusQty");
            DropColumn("dbo.Products", "BonusType");
            DropColumn("dbo.Products", "IsDiscounted");
            DropTable("dbo.ProductBonus");
        }
    }
}