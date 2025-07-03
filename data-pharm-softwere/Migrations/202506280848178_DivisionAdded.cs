namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DivisionAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "VendorID", "dbo.Vendors");
            DropIndex("dbo.Groups", new[] { "VendorID" });
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        DivisionID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        VendorID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DivisionID)
                .ForeignKey("dbo.Vendors", t => t.VendorID, cascadeDelete: true)
                .Index(t => t.VendorID);
            
            AddColumn("dbo.Groups", "DivisionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "DivisionID");
            AddForeignKey("dbo.Groups", "DivisionID", "dbo.Divisions", "DivisionID", cascadeDelete: true);
            DropColumn("dbo.Groups", "VendorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "VendorID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Groups", "DivisionID", "dbo.Divisions");
            DropForeignKey("dbo.Divisions", "VendorID", "dbo.Vendors");
            DropIndex("dbo.Divisions", new[] { "VendorID" });
            DropIndex("dbo.Groups", new[] { "DivisionID" });
            DropColumn("dbo.Groups", "DivisionID");
            DropTable("dbo.Divisions");
            CreateIndex("dbo.Groups", "VendorID");
            AddForeignKey("dbo.Groups", "VendorID", "dbo.Vendors", "VendorID", cascadeDelete: true);
        }
    }
}
