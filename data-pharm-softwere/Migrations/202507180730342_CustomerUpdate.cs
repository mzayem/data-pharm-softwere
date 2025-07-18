namespace data_pharm_softwere.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CustomerUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "ExpiryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "CustomerType", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "PartyType");
        }

        public override void Down()
        {
            AddColumn("dbo.Customers", "PartyType", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "CustomerType");
            DropColumn("dbo.Customers", "ExpiryDate");
        }
    }
}