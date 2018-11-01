namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageMinMaxAddedToArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "MinChar", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "MaxChar", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "Language_LanguageId", c => c.Int());
            CreateIndex("dbo.Article", "Language_LanguageId");
            AddForeignKey("dbo.Article", "Language_LanguageId", "dbo.Language", "LanguageId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Article", "Language_LanguageId", "dbo.Language");
            DropIndex("dbo.Article", new[] { "Language_LanguageId" });
            DropColumn("dbo.Article", "Language_LanguageId");
            DropColumn("dbo.Article", "MaxChar");
            DropColumn("dbo.Article", "MinChar");
        }
    }
}
