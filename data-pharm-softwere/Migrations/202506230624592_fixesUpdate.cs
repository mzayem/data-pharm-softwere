namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class fixesUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "CartonSize", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ReqGST", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "UnReqGST", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            AlterColumn("dbo.Products", "UnReqGST", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ReqGST", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "CartonSize", c => c.String(nullable: false, maxLength: 100));
        }
    }
}