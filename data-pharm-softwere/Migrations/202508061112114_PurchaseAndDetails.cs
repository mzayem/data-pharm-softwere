namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PurchaseAndDetails : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PurchaseDetail", name: "BatchId", newName: "BatchNo");
            RenameIndex(table: "dbo.PurchaseDetail", name: "IX_BatchId", newName: "IX_BatchNo");
            AddColumn("dbo.Purchases", "CreatedBy", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Purchases", "Created_by");
        }

        public override void Down()
        {
            AddColumn("dbo.Purchases", "Created_by", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Purchases", "CreatedBy");
            RenameIndex(table: "dbo.PurchaseDetail", name: "IX_BatchNo", newName: "IX_BatchId");
            RenameColumn(table: "dbo.PurchaseDetail", name: "BatchNo", newName: "BatchId");
        }
    }
}