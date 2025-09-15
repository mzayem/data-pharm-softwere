namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TablesUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountPolicies", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.DiscountPolicies", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.SalesmanTowns", "AssignmentType", c => c.Int(nullable: false));
            AddColumn("dbo.SalesmanTowns", "Percentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.SalesmanTowns", "Percentage");
            DropColumn("dbo.SalesmanTowns", "AssignmentType");
            DropColumn("dbo.DiscountPolicies", "UpdatedAt");
            DropColumn("dbo.DiscountPolicies", "CreatedAt");
        }
    }
}