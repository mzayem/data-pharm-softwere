namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SpellFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "PartyType", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "PartType");
        }

        public override void Down()
        {
            AddColumn("dbo.Customers", "PartType", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "PartyType");
        }
    }
}