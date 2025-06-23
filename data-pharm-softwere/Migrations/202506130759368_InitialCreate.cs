namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vendors",
                c => new
                {
                    VendorID = c.Int(nullable: false, identity: true), // Keep it clean
                    Name = c.String(nullable: false, maxLength: 100),
                    Email = c.String(maxLength: 150),
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
                    MaxDiscountAllowed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PartyType = c.String(maxLength: 50),
                    Remarks = c.String(maxLength: 250),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.VendorID);

            Sql(@"
                DECLARE @currentVendorId BIGINT = IDENT_CURRENT('Vendors');

                IF @currentVendorId IS NULL OR @currentVendorId < 1001
                BEGIN
                    DBCC CHECKIDENT ('Vendors', RESEED, 1001);
                END
            ");
        }

        public override void Down()
        {
            DropTable("dbo.Vendors");
        }
    }
}