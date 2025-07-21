namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinorFixes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Salesmans", newName: "Salesmen");
            AddColumn("dbo.MedicalRepSubGroups", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.MedicalReps", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Salesmen", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salesmen", "CreatedAt");
            DropColumn("dbo.MedicalReps", "CreatedAt");
            DropColumn("dbo.MedicalRepSubGroups", "CreatedAt");
            RenameTable(name: "dbo.Salesmen", newName: "Salesmans");
        }
    }
}
