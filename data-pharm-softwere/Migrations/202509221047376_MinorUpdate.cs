namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesDetail", "BatchStock_BatchStockID", c => c.Int());
            CreateIndex("dbo.SalesDetail", "BatchStock_BatchStockID");
            AddForeignKey("dbo.SalesDetail", "BatchStock_BatchStockID", "dbo.BatchesStock", "BatchStockID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesDetail", "BatchStock_BatchStockID", "dbo.BatchesStock");
            DropIndex("dbo.SalesDetail", new[] { "BatchStock_BatchStockID" });
            DropColumn("dbo.SalesDetail", "BatchStock_BatchStockID");
        }
    }
}
