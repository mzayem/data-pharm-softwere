namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SalesmanModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesmanTowns",
                c => new
                {
                    SalesmanTownID = c.Int(nullable: false, identity: true),
                    SalesmanID = c.Int(nullable: false),
                    TownID = c.Int(nullable: false),
                    AssignedOn = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.SalesmanTownID)
                .ForeignKey("dbo.Salesman", t => t.SalesmanID, cascadeDelete: true)
                .ForeignKey("dbo.Towns", t => t.TownID, cascadeDelete: true)
                .Index(t => t.SalesmanID)
                .Index(t => t.TownID);

            CreateTable(
                "dbo.Salesman",
                c => new
                {
                    SalesmanID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Email = c.String(),
                    Contact = c.String(nullable: false),
                })
                .PrimaryKey(t => t.SalesmanID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.SalesmanTowns", "TownID", "dbo.Towns");
            DropForeignKey("dbo.SalesmanTowns", "SalesmanID", "dbo.Salesman");
            DropIndex("dbo.SalesmanTowns", new[] { "TownID" });
            DropIndex("dbo.SalesmanTowns", new[] { "SalesmanID" });
            DropTable("dbo.Salesman");
            DropTable("dbo.SalesmanTowns");
        }
    }
}