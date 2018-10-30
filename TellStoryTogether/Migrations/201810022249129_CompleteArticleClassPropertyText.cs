namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteArticleClassPropertyText : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "Text");
        }
    }
}
