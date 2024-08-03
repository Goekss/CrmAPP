namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnsprechpartnerRefactor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ansprechpartners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Tel = c.String(maxLength: 40),
                        Mail = c.String(maxLength: 40),
                        Filiale = c.String(),
                        KundenDatenID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KundenDatens", t => t.KundenDatenID, cascadeDelete: true)
                .Index(t => t.KundenDatenID);
            
            DropColumn("dbo.KundenDatens", "Ansprechpartner");
            DropColumn("dbo.KundenDatens", "Antel1");
            DropColumn("dbo.KundenDatens", "Anmail1");
            DropColumn("dbo.KundenDatens", "Anfil1");
            DropColumn("dbo.KundenDatens", "Ansprechpartner2");
            DropColumn("dbo.KundenDatens", "Antel2");
            DropColumn("dbo.KundenDatens", "Anmail2");
            DropColumn("dbo.KundenDatens", "Anfil2");
            DropColumn("dbo.KundenDatens", "Ansprechpartner3");
            DropColumn("dbo.KundenDatens", "Antel3");
            DropColumn("dbo.KundenDatens", "Anmail3");
            DropColumn("dbo.KundenDatens", "Anfil3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KundenDatens", "Anfil3", c => c.String());
            AddColumn("dbo.KundenDatens", "Anmail3", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Antel3", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Ansprechpartner3", c => c.String(maxLength: 25));
            AddColumn("dbo.KundenDatens", "Anfil2", c => c.String());
            AddColumn("dbo.KundenDatens", "Anmail2", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Antel2", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Ansprechpartner2", c => c.String(maxLength: 25));
            AddColumn("dbo.KundenDatens", "Anfil1", c => c.String());
            AddColumn("dbo.KundenDatens", "Anmail1", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Antel1", c => c.String(maxLength: 40));
            AddColumn("dbo.KundenDatens", "Ansprechpartner", c => c.String(maxLength: 25));
            DropForeignKey("dbo.Ansprechpartners", "KundenDatenID", "dbo.KundenDatens");
            DropIndex("dbo.Ansprechpartners", new[] { "KundenDatenID" });
            DropTable("dbo.Ansprechpartners");
        }
    }
}
