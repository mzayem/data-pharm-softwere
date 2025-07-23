namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveClass : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.class1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.class1",
                c => new
                    {
                        BatchID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BatchID);
            
        }
    }
}
