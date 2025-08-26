namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscountAmnt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Dist1", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "Dist2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Dist2");
            DropColumn("dbo.Products", "Dist1");
        }
    }
}
