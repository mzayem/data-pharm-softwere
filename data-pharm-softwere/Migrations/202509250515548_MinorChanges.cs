namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MinorChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesDetail", "BatchStock_BatchStockID", c => c.Int());
            AlterColumn("dbo.Sales", "Remarks", c => c.String(maxLength: 200));
            CreateIndex("dbo.SalesDetail", "BatchStock_BatchStockID");
            AddForeignKey("dbo.SalesDetail", "BatchStock_BatchStockID", "dbo.BatchesStock", "BatchStockID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SalesDetail", "BatchStock_BatchStockID", "dbo.BatchesStock");
            DropIndex("dbo.SalesDetail", new[] { "BatchStock_BatchStockID" });
            AlterColumn("dbo.Sales", "Remarks", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.SalesDetail", "BatchStock_BatchStockID");
        }
    }
}