namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KundenDatens", "PersonalAngaben_PersonalDatenID", c => c.Int());
            CreateIndex("dbo.KundenDatens", "PersonalAngaben_PersonalDatenID");
            AddForeignKey("dbo.KundenDatens", "PersonalAngaben_PersonalDatenID", "dbo.PersonalAngabens", "PersonalDatenID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KundenDatens", "PersonalAngaben_PersonalDatenID", "dbo.PersonalAngabens");
            DropIndex("dbo.KundenDatens", new[] { "PersonalAngaben_PersonalDatenID" });
            DropColumn("dbo.KundenDatens", "PersonalAngaben_PersonalDatenID");
        }
    }
}
