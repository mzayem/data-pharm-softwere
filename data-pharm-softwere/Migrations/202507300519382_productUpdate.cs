namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "DivisionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "DivisionID");
            AddForeignKey("dbo.Products", "DivisionID", "dbo.Divisions", "DivisionID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "DivisionID", "dbo.Divisions");
            DropIndex("dbo.Products", new[] { "DivisionID" });
            DropColumn("dbo.Products", "DivisionID");
        }
    }
}
