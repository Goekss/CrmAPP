namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFailedLoginCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalAngabens", "FailedLoginAttempts", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalAngabens", "FailedLoginAttempts");
        }
    }
}
