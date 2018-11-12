namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentChangeArticleIdToArticle : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comment", name: "ArticleId_ArticleId", newName: "Article_ArticleId");
            RenameIndex(table: "dbo.Comment", name: "IX_ArticleId_ArticleId", newName: "IX_Article_ArticleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Comment", name: "IX_Article_ArticleId", newName: "IX_ArticleId_ArticleId");
            RenameColumn(table: "dbo.Comment", name: "Article_ArticleId", newName: "ArticleId_ArticleId");
        }
    }
}
