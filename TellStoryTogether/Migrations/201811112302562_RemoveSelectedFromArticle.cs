namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSelectedFromArticle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Article", "Selected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Article", "Selected", c => c.Boolean(nullable: false));
        }
    }
}
