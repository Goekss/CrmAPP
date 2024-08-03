namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddBerichtAndRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Berichts",
                c => new
                {
                    BerichtId = c.Int(nullable: false, identity: true),
                    Titel = c.String(maxLength: 100),
                    DateiPfad = c.String(),
                    DateiTuru = c.String(),
                    Version = c.String(maxLength: 10),
                    HochgeladenAm = c.DateTime(nullable: false),
                    BearbeitetAm = c.DateTime(),
                    GeloschtAm = c.DateTime(),
                    HochgeladenVonId = c.Int(),
                    BearbeitetVonId = c.Int(),
                    GeloschtVonId = c.Int(),
                    PerProjID = c.Int(nullable: false),
                    BearbeitetVon_PersonalDatenID = c.Int(),
                    GeloschtVon_PersonalDatenID = c.Int(),
                    HochgeladenVon_PersonalDatenID = c.Int(),
                })
                .PrimaryKey(t => t.BerichtId)
                .ForeignKey("dbo.PersonalAngabens", t => t.BearbeitetVon_PersonalDatenID)
                .ForeignKey("dbo.PersonalAngabens", t => t.GeloschtVon_PersonalDatenID)
                .ForeignKey("dbo.PersonalAngabens", t => t.HochgeladenVon_PersonalDatenID)
                .ForeignKey("dbo.PersonalProjektes", t => t.PerProjID, cascadeDelete: true)
                .Index(t => t.PerProjID)
                .Index(t => t.BearbeitetVon_PersonalDatenID)
                .Index(t => t.GeloschtVon_PersonalDatenID)
                .Index(t => t.HochgeladenVon_PersonalDatenID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Berichts", "PerProjID", "dbo.PersonalProjektes");
            DropForeignKey("dbo.Berichts", "HochgeladenVon_PersonalDatenID", "dbo.PersonalAngabens");
            DropForeignKey("dbo.Berichts", "GeloschtVon_PersonalDatenID", "dbo.PersonalAngabens");
            DropForeignKey("dbo.Berichts", "BearbeitetVon_PersonalDatenID", "dbo.PersonalAngabens");
            DropIndex("dbo.Berichts", new[] { "HochgeladenVon_PersonalDatenID" });
            DropIndex("dbo.Berichts", new[] { "GeloschtVon_PersonalDatenID" });
            DropIndex("dbo.Berichts", new[] { "BearbeitetVon_PersonalDatenID" });
            DropIndex("dbo.Berichts", new[] { "PerProjID" });
            DropTable("dbo.Berichts");
        }
    }
}
