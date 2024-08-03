namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelGuncelleme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalProjektes", "SelectedKundenDatenId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalProjektes", "SelectedKundenDatenId");
        }
    }
}
