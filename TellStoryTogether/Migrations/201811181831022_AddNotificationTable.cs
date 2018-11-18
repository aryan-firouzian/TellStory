namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Content = c.String(),
                        State = c.String(),
                        Seen = c.Boolean(nullable: false),
                        Starred = c.Int(nullable: false),
                        Favorited = c.Int(nullable: false),
                        Commented = c.Int(nullable: false),
                        Forked = c.Int(nullable: false),
                        Article_ArticleId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Article", t => t.Article_ArticleId)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.Article_ArticleId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Notification", "Article_ArticleId", "dbo.Article");
            DropIndex("dbo.Notification", new[] { "User_UserId" });
            DropIndex("dbo.Notification", new[] { "Article_ArticleId" });
            DropTable("dbo.Notification");
        }
    }
}
