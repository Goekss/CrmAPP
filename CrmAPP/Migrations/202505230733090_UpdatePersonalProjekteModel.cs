namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePersonalProjekteModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ansprechpartners", "PersonalProjekte_PerProjID", c => c.Int());
            CreateIndex("dbo.Ansprechpartners", "PersonalProjekte_PerProjID");
            AddForeignKey("dbo.Ansprechpartners", "PersonalProjekte_PerProjID", "dbo.PersonalProjektes", "PerProjID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ansprechpartners", "PersonalProjekte_PerProjID", "dbo.PersonalProjektes");
            DropIndex("dbo.Ansprechpartners", new[] { "PersonalProjekte_PerProjID" });
            DropColumn("dbo.Ansprechpartners", "PersonalProjekte_PerProjID");
        }
    }
}
