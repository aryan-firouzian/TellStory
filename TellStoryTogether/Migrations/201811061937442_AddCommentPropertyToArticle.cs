namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentPropertyToArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Comment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "Comment");
        }
    }
}
