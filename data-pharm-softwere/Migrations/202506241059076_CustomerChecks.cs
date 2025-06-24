namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CustomerChecks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "NorcoticsSaleAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "InActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "IsAdvTaxExempted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "FbrInActiveGST", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "FBRInActiveTax236H", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Customers", "FBRInActiveTax236H");
            DropColumn("dbo.Customers", "FbrInActiveGST");
            DropColumn("dbo.Customers", "IsAdvTaxExempted");
            DropColumn("dbo.Customers", "InActive");
            DropColumn("dbo.Customers", "NorcoticsSaleAllowed");
        }
    }
}