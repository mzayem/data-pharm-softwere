using System.Data.Entity.Migrations;

namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TownUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Towns", name: "RouteID", newName: "CityRouteID");
            RenameIndex(table: "dbo.Towns", name: "IX_RouteID", newName: "IX_CityRouteID");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.Towns", name: "IX_CityRouteID", newName: "IX_RouteID");
            RenameColumn(table: "dbo.Towns", name: "CityRouteID", newName: "RouteID");
        }
    }
}