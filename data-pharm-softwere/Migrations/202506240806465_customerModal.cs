namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class customerModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                {
                    CustomerId = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    Email = c.String(maxLength: 150),
                    Contact = c.String(nullable: false, maxLength: 50),
                    CNIC = c.String(nullable: false, maxLength: 50),
                    Address = c.String(nullable: false, maxLength: 250),
                    TownID = c.Int(nullable: false),
                    LicenceNo = c.String(maxLength: 50),
                    PartType = c.Int(nullable: false),
                    NtnNo = c.String(nullable: false, maxLength: 20),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Towns", t => t.TownID, cascadeDelete: true)
                .Index(t => t.TownID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Customers", "TownID", "dbo.Towns");
            DropIndex("dbo.Customers", new[] { "TownID" });
            DropTable("dbo.Customers");
        }
    }
}