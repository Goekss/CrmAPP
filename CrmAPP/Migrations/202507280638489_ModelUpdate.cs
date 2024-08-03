namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.PersonalAngabens SET Rol = 'Standard' WHERE Rol IS NULL");
            AlterColumn("dbo.PersonalAngabens", "Vorname", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.PersonalAngabens", "Nachname", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.PersonalAngabens", "Bild", c => c.String(nullable: false));
            AlterColumn("dbo.PersonalAngabens", "Email", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.PersonalAngabens", "Passwort", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.PersonalAngabens", "RufNummer", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.PersonalAngabens", "Abteilung", c => c.String(nullable: false));
            AlterColumn("dbo.PersonalAngabens", "Aufgaben", c => c.String(nullable: false));
            AlterColumn("dbo.PersonalAngabens", "Berechtigung", c => c.String(nullable: false));
            AlterColumn("dbo.PersonalAngabens", "Rol", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.PersonalAngabens", "Hinweise", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonalAngabens", "Hinweise", c => c.String());
            AlterColumn("dbo.PersonalAngabens", "Rol", c => c.String(maxLength: 50));
            AlterColumn("dbo.PersonalAngabens", "Berechtigung", c => c.String());
            AlterColumn("dbo.PersonalAngabens", "Aufgaben", c => c.String());
            AlterColumn("dbo.PersonalAngabens", "Abteilung", c => c.String());
            AlterColumn("dbo.PersonalAngabens", "RufNummer", c => c.String(maxLength: 20));
            AlterColumn("dbo.PersonalAngabens", "Passwort", c => c.String(maxLength: 64));
            AlterColumn("dbo.PersonalAngabens", "Email", c => c.String(maxLength: 25));
            AlterColumn("dbo.PersonalAngabens", "Bild", c => c.String());
            AlterColumn("dbo.PersonalAngabens", "Nachname", c => c.String(maxLength: 30));
            AlterColumn("dbo.PersonalAngabens", "Vorname", c => c.String(maxLength: 30));
        }
    }
}
