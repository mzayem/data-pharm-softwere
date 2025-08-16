namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CityRouteTownModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CityRoutes",
                c => new
                {
                    CityRouteID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.CityRouteID);

            CreateTable(
                "dbo.Towns",
                c => new
                {
                    TownID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    CityRouteID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.TownID)
                .ForeignKey("dbo.CityRoutes", t => t.CityRouteID, cascadeDelete: true)
                .Index(t => t.CityRouteID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Towns", "CityRouteID", "dbo.CityRoutes");
            DropIndex("dbo.Towns", new[] { "CityRouteID" });
            DropTable("dbo.Towns");
            DropTable("dbo.CityRoutes");
        }
    }
}