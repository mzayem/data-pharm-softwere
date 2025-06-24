namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TownCityRouteModal : DbMigration
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
                    RouteID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.TownID)
                .ForeignKey("dbo.CityRoutes", t => t.RouteID, cascadeDelete: true)
                .Index(t => t.RouteID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Towns", "RouteID", "dbo.CityRoutes");
            DropIndex("dbo.Towns", new[] { "RouteID" });
            DropTable("dbo.Towns");
            DropTable("dbo.CityRoutes");
        }
    }
}