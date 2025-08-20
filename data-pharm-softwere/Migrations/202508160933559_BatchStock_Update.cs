namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BatchStock_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchesStock", "OnHoldQty", c => c.Int(nullable: false));
            AddColumn("dbo.BatchesStock", "QuarantineQty", c => c.Int(nullable: false));
            DropColumn("dbo.BatchesStock", "UnavailableQty");
        }

        public override void Down()
        {
            AddColumn("dbo.BatchesStock", "UnavailableQty", c => c.Int(nullable: false));
            DropColumn("dbo.BatchesStock", "QuarantineQty");
            DropColumn("dbo.BatchesStock", "OnHoldQty");
        }
    }
}