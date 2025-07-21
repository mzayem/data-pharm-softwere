namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MedicalReps : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Salesman", newName: "Salesmans");
            CreateTable(
                "dbo.MedicalRepSubGroups",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    MedicalRepID = c.Int(nullable: false),
                    SubGroupID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MedicalReps", t => t.MedicalRepID, cascadeDelete: true)
                .ForeignKey("dbo.SubGroups", t => t.SubGroupID, cascadeDelete: true)
                .Index(t => t.MedicalRepID)
                .Index(t => t.SubGroupID);

            CreateTable(
                "dbo.MedicalReps",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Email = c.String(),
                    Contact = c.String(nullable: false),
                    Type = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            AlterColumn("dbo.Vendors", "Email", c => c.String());
            AlterColumn("dbo.Customers", "Email", c => c.String());
        }

        public override void Down()
        {
            DropForeignKey("dbo.MedicalRepSubGroups", "SubGroupID", "dbo.SubGroups");
            DropForeignKey("dbo.MedicalRepSubGroups", "MedicalRepID", "dbo.MedicalReps");
            DropIndex("dbo.MedicalRepSubGroups", new[] { "SubGroupID" });
            DropIndex("dbo.MedicalRepSubGroups", new[] { "MedicalRepID" });
            AlterColumn("dbo.Customers", "Email", c => c.String(maxLength: 150));
            AlterColumn("dbo.Vendors", "Email", c => c.String(maxLength: 150));
            DropTable("dbo.MedicalReps");
            DropTable("dbo.MedicalRepSubGroups");
            RenameTable(name: "dbo.Salesmans", newName: "Salesman");
        }
    }
}