namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class VoucherHeads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "VoucherNumber", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Purchases", "VoucherType", c => c.Int(nullable: false));
            AddColumn("dbo.Settings", "PurchaseHead", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Settings", "PurchaseReturnHead", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Settings", "TransferInHead", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Settings", "TransferOutHead", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Settings", "SalesHead", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Settings", "SalesReturnHead", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Purchases", "PurchaseType");
        }

        public override void Down()
        {
            AddColumn("dbo.Purchases", "PurchaseType", c => c.Int(nullable: false));
            DropColumn("dbo.Settings", "SalesReturnHead");
            DropColumn("dbo.Settings", "SalesHead");
            DropColumn("dbo.Settings", "TransferOutHead");
            DropColumn("dbo.Settings", "TransferInHead");
            DropColumn("dbo.Settings", "PurchaseReturnHead");
            DropColumn("dbo.Settings", "PurchaseHead");
            DropColumn("dbo.Purchases", "VoucherType");
            DropColumn("dbo.Purchases", "VoucherNumber");
        }
    }
}