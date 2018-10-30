namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    // Add-Migration name in the NuGet Command
    public partial class AddBlogRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Article",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        ArticleInitId = c.Int(nullable: false),
                        Selected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId);
        
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.UserProfile");
            //DropTable("dbo.Article");
        }
    }
}
