namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleRemoveSinPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Point", c => c.Int(nullable: false));
            DropColumn("dbo.Article", "Points");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Article", "Points", c => c.Int(nullable: false));
            DropColumn("dbo.Article", "Point");
        }
    }
}
