namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ProductUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Products", "VendorID", "dbo.Vendors");
            DropIndex("dbo.Products", new[] { "VendorID" });
            DropIndex("dbo.Products", new[] { "GroupID" });
            AddColumn("dbo.Products", "SubGroupID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "SubGroupID");
            AddForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups", "SubGroupID");
            DropColumn("dbo.Products", "VendorID");
            DropColumn("dbo.Products", "GroupID");
        }

        public override void Down()
        {
            AddColumn("dbo.Products", "GroupID", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "VendorID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups");
            DropIndex("dbo.Products", new[] { "SubGroupID" });
            DropColumn("dbo.Products", "SubGroupID");
            CreateIndex("dbo.Products", "GroupID");
            CreateIndex("dbo.Products", "VendorID");
            AddForeignKey("dbo.Products", "VendorID", "dbo.Vendors", "VendorID");
            AddForeignKey("dbo.Products", "GroupID", "dbo.Groups", "GroupID");
        }
    }
}