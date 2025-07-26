namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AccountChartUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Divisions", "VendorID", "dbo.Vendors");
            RenameColumn(table: "dbo.Divisions", name: "VendorID", newName: "AccountId");
            RenameIndex(table: "dbo.Divisions", name: "IX_VendorID", newName: "IX_AccountId");

            //DropPrimaryKey("dbo.Accounts");
            DropPrimaryKey("dbo.Vendors");

            AddColumn("dbo.Accounts", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Vendors", "AccountId", c => c.Int(nullable: false));

            AlterColumn("dbo.Accounts", "AccountId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Accounts", "AccountName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Accounts", "AccountType", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Accounts", "AccountDetail", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Accounts", "Status", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Accounts", "salary_s", c => c.String(nullable: false, maxLength: 100));

            AddPrimaryKey("dbo.Accounts", "AccountId");
            AddPrimaryKey("dbo.Vendors", "AccountId");

            CreateIndex("dbo.Vendors", "AccountId");
            AddForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
            AddForeignKey("dbo.Divisions", "AccountId", "dbo.Vendors", "AccountId", cascadeDelete: true);

            DropColumn("dbo.Vendors", "VendorID");
            DropColumn("dbo.Vendors", "Name");
            DropColumn("dbo.Vendors", "MaxDiscountAllowed");
        }

        public override void Down()
        {
            AddColumn("dbo.Vendors", "MaxDiscountAllowed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Vendors", "Name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Vendors", "VendorID", c => c.Int(nullable: false, identity: true));

            DropForeignKey("dbo.Divisions", "AccountId", "dbo.Vendors");
            DropForeignKey("dbo.Vendors", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Vendors", new[] { "AccountId" });

            DropPrimaryKey("dbo.Vendors");
            DropPrimaryKey("dbo.Accounts");

            AlterColumn("dbo.Accounts", "salary_s", c => c.String());
            AlterColumn("dbo.Accounts", "Status", c => c.String());
            AlterColumn("dbo.Accounts", "AccountDetail", c => c.String());
            AlterColumn("dbo.Accounts", "AccountType", c => c.String());
            AlterColumn("dbo.Accounts", "AccountName", c => c.String());
            AlterColumn("dbo.Accounts", "AccountId", c => c.Double(nullable: false));

            DropColumn("dbo.Vendors", "AccountId");
            DropColumn("dbo.Accounts", "CreatedAt");

            AddPrimaryKey("dbo.Vendors", "VendorID");
            AddPrimaryKey("dbo.Accounts", "AccountId");

            RenameIndex(table: "dbo.Divisions", name: "IX_AccountId", newName: "IX_VendorID");
            RenameColumn(table: "dbo.Divisions", name: "AccountId", newName: "VendorID");

            AddForeignKey("dbo.Divisions", "VendorID", "dbo.Vendors", "VendorID", cascadeDelete: true);
        }
    }
}