namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GroupSubGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                {
                    GroupID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    VendorID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.GroupID)
                .ForeignKey("dbo.Vendors", t => t.VendorID, cascadeDelete: true)
                .Index(t => t.VendorID);

            CreateTable(
                "dbo.SubGroups",
                c => new
                {
                    SubGroupID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    GroupID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.SubGroupID)
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Groups", "VendorID", "dbo.Vendors");
            DropForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups");
            DropIndex("dbo.SubGroups", new[] { "GroupID" });
            DropIndex("dbo.Groups", new[] { "VendorID" });
            DropTable("dbo.SubGroups");
            DropTable("dbo.Groups");
        }
    }
}