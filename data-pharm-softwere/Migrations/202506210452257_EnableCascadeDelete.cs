namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EnableCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "VendorID", "dbo.Vendors");
            DropForeignKey("dbo.Batches", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups");
            AddColumn("dbo.Products", "SubGroup_SubGroupID", c => c.Int());
            CreateIndex("dbo.Products", "SubGroup_SubGroupID");
            AddForeignKey("dbo.Products", "SubGroup_SubGroupID", "dbo.SubGroups", "SubGroupID");
            AddForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups", "GroupID", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "VendorID", "dbo.Vendors", "VendorID", cascadeDelete: true);
            AddForeignKey("dbo.Batches", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups", "SubGroupID", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups");
            DropForeignKey("dbo.Batches", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Groups", "VendorID", "dbo.Vendors");
            DropForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Products", "SubGroup_SubGroupID", "dbo.SubGroups");
            DropIndex("dbo.Products", new[] { "SubGroup_SubGroupID" });
            DropColumn("dbo.Products", "SubGroup_SubGroupID");
            AddForeignKey("dbo.Products", "SubGroupID", "dbo.SubGroups", "SubGroupID");
            AddForeignKey("dbo.Batches", "ProductID", "dbo.Products", "ProductID");
            AddForeignKey("dbo.Groups", "VendorID", "dbo.Vendors", "VendorID");
            AddForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups", "GroupID");
        }
    }
}