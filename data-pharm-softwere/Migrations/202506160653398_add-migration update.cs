namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addmigrationupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "VendorID", "dbo.Vendors");
            DropForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups");
            AddColumn("dbo.Groups", "Vendor_VendorID", c => c.Int());
            CreateIndex("dbo.Groups", "Vendor_VendorID");
            AddForeignKey("dbo.Groups", "Vendor_VendorID", "dbo.Vendors", "VendorID");
            AddForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups", "GroupID");
            AddForeignKey("dbo.Groups", "VendorID", "dbo.Vendors", "VendorID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Groups", "VendorID", "dbo.Vendors");
            DropForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Vendor_VendorID", "dbo.Vendors");
            DropIndex("dbo.Groups", new[] { "Vendor_VendorID" });
            DropColumn("dbo.Groups", "Vendor_VendorID");
            AddForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups", "GroupID", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "VendorID", "dbo.Vendors", "VendorID", cascadeDelete: true);
        }
    }
}