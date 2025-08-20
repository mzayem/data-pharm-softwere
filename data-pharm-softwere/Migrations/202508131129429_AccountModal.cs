namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AccountModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                {
                    AccountId = c.Int(nullable: false, identity: false),
                    AccountName = c.String(nullable: false, maxLength: 100),
                    AccountType = c.String(nullable: false, maxLength: 100),
                    AccountDetail = c.String(nullable: false, maxLength: 100),
                    Status = c.String(nullable: false, maxLength: 100),
                    salary_s = c.String(nullable: false, maxLength: 100),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.AccountId);
        }

        public override void Down()
        {
            DropTable("dbo.Accounts");
        }
    }
}