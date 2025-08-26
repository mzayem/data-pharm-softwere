namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class DataTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.data",
                c => new
                {
                    Sr = c.Int(name: "Sr#", nullable: false, identity: true),
                    Vr = c.Int(name: "Vr#"),
                    VrDate = c.DateTime(storeType: "date"),
                    AccountId = c.Int(),
                    AccountTitle = c.String(maxLength: 255),
                    Dr = c.Decimal(storeType: "money"),
                    Cr = c.Decimal(storeType: "money"),
                    Remarks = c.String(maxLength: 255),
                    Type = c.String(maxLength: 255),
                    Status = c.String(maxLength: 255),
                    Creator = c.String(maxLength: 255),
                    Vtype = c.String(maxLength: 100),
                    Updater = c.String(maxLength: 255),
                    Updetail = c.String(maxLength: 255),
                    recamt = c.Decimal(storeType: "money"),
                })
                .PrimaryKey(t => t.Sr);

            CreateTable(
                "dbo.Settings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(maxLength: 100),
                    DefaultCurrency = c.String(maxLength: 50),
                    Address = c.String(maxLength: 200),
                    StockInHandAccountNo = c.String(nullable: false, maxLength: 50),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Settings");
            DropTable("dbo.data");
        }
    }
}