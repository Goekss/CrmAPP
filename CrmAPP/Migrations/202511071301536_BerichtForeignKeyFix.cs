namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BerichtForeignKeyFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Berichts", "BearbeitetVonId");
            DropColumn("dbo.Berichts", "GeloschtVonId");
            DropColumn("dbo.Berichts", "HochgeladenVonId");
            RenameColumn(table: "dbo.Berichts", name: "BearbeitetVon_PersonalDatenID", newName: "BearbeitetVonId");
            RenameColumn(table: "dbo.Berichts", name: "GeloschtVon_PersonalDatenID", newName: "GeloschtVonId");
            RenameColumn(table: "dbo.Berichts", name: "HochgeladenVon_PersonalDatenID", newName: "HochgeladenVonId");
            RenameIndex(table: "dbo.Berichts", name: "IX_HochgeladenVon_PersonalDatenID", newName: "IX_HochgeladenVonId");
            RenameIndex(table: "dbo.Berichts", name: "IX_BearbeitetVon_PersonalDatenID", newName: "IX_BearbeitetVonId");
            RenameIndex(table: "dbo.Berichts", name: "IX_GeloschtVon_PersonalDatenID", newName: "IX_GeloschtVonId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Berichts", name: "IX_GeloschtVonId", newName: "IX_GeloschtVon_PersonalDatenID");
            RenameIndex(table: "dbo.Berichts", name: "IX_BearbeitetVonId", newName: "IX_BearbeitetVon_PersonalDatenID");
            RenameIndex(table: "dbo.Berichts", name: "IX_HochgeladenVonId", newName: "IX_HochgeladenVon_PersonalDatenID");
            RenameColumn(table: "dbo.Berichts", name: "HochgeladenVonId", newName: "HochgeladenVon_PersonalDatenID");
            RenameColumn(table: "dbo.Berichts", name: "GeloschtVonId", newName: "GeloschtVon_PersonalDatenID");
            RenameColumn(table: "dbo.Berichts", name: "BearbeitetVonId", newName: "BearbeitetVon_PersonalDatenID");
            AddColumn("dbo.Berichts", "HochgeladenVonId", c => c.Int());
            AddColumn("dbo.Berichts", "GeloschtVonId", c => c.Int());
            AddColumn("dbo.Berichts", "BearbeitetVonId", c => c.Int());
        }
    }
}
