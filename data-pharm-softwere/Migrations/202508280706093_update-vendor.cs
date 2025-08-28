namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class updatevendor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "AdvTaxRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.Vendors", "AdvTaxRate");
        }
    }
}