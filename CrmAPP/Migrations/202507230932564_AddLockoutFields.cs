namespace CrmAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLockoutFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalAngabens", "IsLockedOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.PersonalAngabens", "LockoutTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalAngabens", "LockoutTime");
            DropColumn("dbo.PersonalAngabens", "IsLockedOut");
        }
    }
}
