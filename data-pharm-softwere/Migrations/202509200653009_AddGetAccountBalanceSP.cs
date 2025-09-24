namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddGetAccountBalanceSP : DbMigration
    {
        public override void Up()
        {
            Sql(@"
        CREATE PROCEDURE dbo.GetAccountBalance
            @AccountId INT
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT
                ISNULL(SUM(Dr), 0) - ISNULL(SUM(Cr), 0) AS NetBalance
            FROM Data
            WHERE AccountId = @AccountId;
        END
    ");
        }

        public override void Down()
        {
            Sql("DROP PROCEDURE dbo.GetAccountBalance");
        }
    }
}