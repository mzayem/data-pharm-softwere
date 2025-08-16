namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class VendorModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vendors",
                c => new
                {
                    AccountId = c.Int(nullable: false),
                    Email = c.String(),
                    Contact = c.String(maxLength: 50),
                    Address = c.String(nullable: false, maxLength: 250),
                    Town = c.String(nullable: false, maxLength: 100),
                    City = c.String(nullable: false, maxLength: 100),
                    LicenceNo = c.String(maxLength: 50),
                    ExpiryDate = c.DateTime(nullable: false),
                    SRACode = c.String(nullable: false, maxLength: 20),
                    GstNo = c.String(nullable: false, maxLength: 20),
                    NtnNo = c.String(nullable: false, maxLength: 20),
                    CompanyCode = c.String(nullable: false, maxLength: 20),
                    Remarks = c.String(maxLength: 250),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Vendors", new[] { "AccountId" });
            DropTable("dbo.Vendors");
        }
    }
}