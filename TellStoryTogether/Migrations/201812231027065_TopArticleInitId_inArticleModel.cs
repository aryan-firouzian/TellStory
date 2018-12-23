namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TopArticleInitId_inArticleModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "TopArticleInitId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "TopArticleInitId");
        }
    }
}
