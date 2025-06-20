namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "CompanyCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CompanyCode", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
