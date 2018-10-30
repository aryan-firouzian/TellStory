namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleUpdateWithSeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Title", c => c.String());
            AddColumn("dbo.Article", "Points", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "Seen", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "Favorite", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "PictureUrl", c => c.String());
            AddColumn("dbo.Article", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "Time");
            DropColumn("dbo.Article", "PictureUrl");
            DropColumn("dbo.Article", "Favorite");
            DropColumn("dbo.Article", "Seen");
            DropColumn("dbo.Article", "Points");
            DropColumn("dbo.Article", "Title");
        }
    }
}
