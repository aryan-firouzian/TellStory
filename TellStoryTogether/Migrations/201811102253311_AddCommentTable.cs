namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        ArticleId_ArticleId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Article", t => t.ArticleId_ArticleId)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.ArticleId_ArticleId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Comment", "ArticleId_ArticleId", "dbo.Article");
            DropIndex("dbo.Comment", new[] { "User_UserId" });
            DropIndex("dbo.Comment", new[] { "ArticleId_ArticleId" });
            DropTable("dbo.Comment");
        }
    }
}
