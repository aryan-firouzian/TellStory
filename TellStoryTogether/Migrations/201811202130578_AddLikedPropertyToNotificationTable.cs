namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLikedPropertyToNotificationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Liked", c => c.Int(nullable: false));
            DropColumn("dbo.Notification", "Starred");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notification", "Starred", c => c.Int(nullable: false));
            DropColumn("dbo.Notification", "Liked");
        }
    }
}
