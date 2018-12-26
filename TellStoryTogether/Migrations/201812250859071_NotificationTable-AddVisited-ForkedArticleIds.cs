namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationTableAddVisitedForkedArticleIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Visited", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notification", "ForkedArticleIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "ForkedArticleIds");
            DropColumn("dbo.Notification", "Visited");
        }
    }
}
