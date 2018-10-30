namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenreAddedToArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Genre_GenreId", c => c.Int());
            CreateIndex("dbo.Article", "Genre_GenreId");
            AddForeignKey("dbo.Article", "Genre_GenreId", "dbo.Genre", "GenreId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Article", "Genre_GenreId", "dbo.Genre");
            DropIndex("dbo.Article", new[] { "Genre_GenreId" });
            DropColumn("dbo.Article", "Genre_GenreId");
        }
    }
}
