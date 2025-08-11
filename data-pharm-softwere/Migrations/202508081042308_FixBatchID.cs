namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixBatchID : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Batches", new[] { "ProductID" });
            RenameColumn(table: "dbo.PurchaseDetail", name: "BatchNo", newName: "BatchID");
            RenameIndex(table: "dbo.PurchaseDetail", name: "IX_BatchNo", newName: "IX_BatchID");
            AlterColumn("dbo.Batches", "BatchNo", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Batches", "ProductID", c => c.Int(nullable: false));
            CreateIndex("dbo.Batches", "ProductID");
        }

        public override void Down()
        {
            DropIndex("dbo.Batches", new[] { "ProductID" });
            AlterColumn("dbo.Batches", "ProductID", c => c.Int());
            AlterColumn("dbo.Batches", "BatchNo", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.PurchaseDetail", name: "IX_BatchID", newName: "IX_BatchNo");
            RenameColumn(table: "dbo.PurchaseDetail", name: "BatchID", newName: "BatchNo");
            CreateIndex("dbo.Batches", "ProductID");
        }
    }
}