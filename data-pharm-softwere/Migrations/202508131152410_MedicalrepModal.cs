namespace data_pharm_softwere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MedicalrepModal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalRepSubGroups",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    MedicalRepID = c.Int(nullable: false),
                    SubGroupID = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
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
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.MedicalRepSubGroups", "SubGroupID", "dbo.SubGroups");
            DropForeignKey("dbo.MedicalRepSubGroups", "MedicalRepID", "dbo.MedicalReps");
            DropIndex("dbo.MedicalRepSubGroups", new[] { "SubGroupID" });
            DropIndex("dbo.MedicalRepSubGroups", new[] { "MedicalRepID" });
            DropTable("dbo.MedicalReps");
            DropTable("dbo.MedicalRepSubGroups");
        }
    }
}