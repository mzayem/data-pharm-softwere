namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.BatchesStock", "ProductID", "dbo.Products");
            DropPrimaryKey("dbo.Accounts");
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.Accounts", "AccountId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ProductID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Accounts", "AccountId");
            AddPrimaryKey("dbo.Products", "ProductID");
            AddForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
            AddForeignKey("dbo.BatchesStock", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.BatchesStock", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts");
            DropPrimaryKey("dbo.Products");
            DropPrimaryKey("dbo.Accounts");
            AlterColumn("dbo.Products", "ProductID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Accounts", "AccountId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Products", "ProductID");
            AddPrimaryKey("dbo.Accounts", "AccountId");
            AddForeignKey("dbo.BatchesStock", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
        }
    }
}