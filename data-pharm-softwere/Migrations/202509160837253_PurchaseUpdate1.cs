namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PurchaseUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseDetail", "Qty", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseDetail", "BonusQty", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseDetail", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PurchaseDetail", "GSTAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.PurchaseDetail", "GSTAmount");
            DropColumn("dbo.PurchaseDetail", "DiscountAmount");
            DropColumn("dbo.PurchaseDetail", "BonusQty");
            DropColumn("dbo.PurchaseDetail", "Qty");
        }
    }
}