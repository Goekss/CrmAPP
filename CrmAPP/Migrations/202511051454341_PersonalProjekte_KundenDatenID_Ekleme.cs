namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonalProjekte_KundenDatenID_Ekleme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalProjektes", "KundenDatenID", c => c.Int());
            DropColumn("dbo.PersonalProjektes", "SelectedKundenDatenId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonalProjektes", "SelectedKundenDatenId", c => c.Int());
            DropColumn("dbo.PersonalProjektes", "KundenDatenID");
        }
    }
}
