namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KundenDatens", "Ansprechpartner", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KundenDatens", "Ansprechpartner", c => c.String());
        }
    }
}
