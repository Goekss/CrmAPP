namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBerichtModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Berichts", "HochgeladenAm", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Berichts", "HochgeladenAm", c => c.DateTime(nullable: false));
        }
    }
}
