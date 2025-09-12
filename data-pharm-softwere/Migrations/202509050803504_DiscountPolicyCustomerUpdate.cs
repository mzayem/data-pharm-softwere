namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class DiscountPolicyCustomerUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Customers");
            CreateTable(
                "dbo.DiscountPolicies",
                c => new
                {
                    DiscountPolicyID = c.Int(nullable: false, identity: true),
                    ProductID = c.Int(nullable: false),
                    CustomerAccountId = c.Int(nullable: false),
                    FlatDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    FlatDiscountExpiry = c.DateTime(),
                    SpecialDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SpecialDiscountExpiry = c.DateTime(),
                })
                .PrimaryKey(t => t.DiscountPolicyID)
                .ForeignKey("dbo.Customers", t => t.CustomerAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.ProductID)
                .Index(t => t.CustomerAccountId);

            AddColumn("dbo.Customers", "AccountId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Customers", "AccountId");
            CreateIndex("dbo.Customers", "AccountId");
            AddForeignKey("dbo.Customers", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
            DropColumn("dbo.Customers", "CustomerId");
            DropColumn("dbo.Customers", "Name");
        }

        public override void Down()
        {
            AddColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Customers", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.DiscountPolicies", "ProductID", "dbo.Products");
            DropForeignKey("dbo.DiscountPolicies", "CustomerAccountId", "dbo.Customers");
            DropIndex("dbo.DiscountPolicies", new[] { "CustomerAccountId" });
            DropIndex("dbo.DiscountPolicies", new[] { "ProductID" });
            DropIndex("dbo.Customers", new[] { "AccountId" });
            DropPrimaryKey("dbo.Customers");
            DropColumn("dbo.Customers", "AccountId");
            DropTable("dbo.DiscountPolicies");
            AddPrimaryKey("dbo.Customers", "CustomerId");
        }
    }
}