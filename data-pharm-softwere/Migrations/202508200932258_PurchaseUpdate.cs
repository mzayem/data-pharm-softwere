namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PurchaseUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "Reference", c => c.String(nullable: false, maxLength: 200));
        }

        public override void Down()
        {
            AlterColumn("dbo.Purchases", "Reference", c => c.String(maxLength: 200));
        }
    }
}