namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMultipleAnsprechpartnerFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KundenDatens", "Antel1", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Anmail1", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Anfil1", c => c.String());
            AddColumn("dbo.KundenDatens", "Ansprechpartner2", c => c.String(maxLength: 25));
            AddColumn("dbo.KundenDatens", "Antel2", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Anmail2", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Anfil2", c => c.String());
            AddColumn("dbo.KundenDatens", "Ansprechpartner3", c => c.String(maxLength: 25));
            AddColumn("dbo.KundenDatens", "Antel3", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Anmail3", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Anfil3", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KundenDatens", "Anfil3");
            DropColumn("dbo.KundenDatens", "Anmail3");
            DropColumn("dbo.KundenDatens", "Antel3");
            DropColumn("dbo.KundenDatens", "Ansprechpartner3");
            DropColumn("dbo.KundenDatens", "Anfil2");
            DropColumn("dbo.KundenDatens", "Anmail2");
            DropColumn("dbo.KundenDatens", "Antel2");
            DropColumn("dbo.KundenDatens", "Ansprechpartner2");
            DropColumn("dbo.KundenDatens", "Anfil1");
            DropColumn("dbo.KundenDatens", "Anmail1");
            DropColumn("dbo.KundenDatens", "Antel1");
        }
    }
}
