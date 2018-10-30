namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteArticleClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Serial", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "Parallel", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "Owner_UserId", c => c.Int());
            CreateIndex("dbo.Article", "Owner_UserId");
            AddForeignKey("dbo.Article", "Owner_UserId", "dbo.UserProfile", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Article", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.Article", new[] { "Owner_UserId" });
            DropColumn("dbo.Article", "Owner_UserId");
            DropColumn("dbo.Article", "Parallel");
            DropColumn("dbo.Article", "Serial");
        }
    }
}
