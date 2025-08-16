namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DivisionGroupSubGroupModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        DivisionID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        AccountId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DivisionID)
                .ForeignKey("dbo.Vendors", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        DivisionID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupID)
                .ForeignKey("dbo.Divisions", t => t.DivisionID, cascadeDelete: true)
                .Index(t => t.DivisionID);
            
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
            DropForeignKey("dbo.Divisions", "AccountId", "dbo.Vendors");
            DropForeignKey("dbo.SubGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "DivisionID", "dbo.Divisions");
            DropIndex("dbo.SubGroups", new[] { "GroupID" });
            DropIndex("dbo.Groups", new[] { "DivisionID" });
            DropIndex("dbo.Divisions", new[] { "AccountId" });
            DropTable("dbo.SubGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.Divisions");
        }
    }
}
