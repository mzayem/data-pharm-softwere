namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassCreation : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.class1");
        }
    }
}
