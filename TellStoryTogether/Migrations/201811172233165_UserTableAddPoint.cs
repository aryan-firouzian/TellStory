namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTableAddPoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "UserPoint", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "UserPoint");
        }
    }
}
