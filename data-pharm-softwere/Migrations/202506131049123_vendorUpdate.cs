namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class vendorUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vendors", "PartyType");
        }

        public override void Down()
        {
            AddColumn("dbo.Vendors", "PartyType", c => c.String(maxLength: 50));
        }
    }
}