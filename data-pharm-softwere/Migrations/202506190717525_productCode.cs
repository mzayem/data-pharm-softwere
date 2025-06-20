namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class productCode : DbMigration
{
    public override void Up()
    {
        AlterColumn("dbo.Products", "ProductCode", c => c.String(nullable: false, maxLength: 50));
    }

    public override void Down()
    {
        AlterColumn("dbo.Products", "ProductCode", c => c.Int(nullable: false));
    }
}
}
