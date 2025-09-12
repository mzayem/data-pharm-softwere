namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountPolicies", "CreditDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DiscountPolicies", "CreditDiscountExpiry", c => c.DateTime());
            DropColumn("dbo.DiscountPolicies", "SpecialDiscount");
            DropColumn("dbo.DiscountPolicies", "SpecialDiscountExpiry");
        }

        public override void Down()
        {
            AddColumn("dbo.DiscountPolicies", "SpecialDiscountExpiry", c => c.DateTime());
            AddColumn("dbo.DiscountPolicies", "SpecialDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.DiscountPolicies", "CreditDiscountExpiry");
            DropColumn("dbo.DiscountPolicies", "CreditDiscount");
        }
    }
}