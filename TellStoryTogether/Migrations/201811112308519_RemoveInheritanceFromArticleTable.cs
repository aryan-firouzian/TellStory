namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveInheritanceFromArticleTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Article", "Pointed");
            DropColumn("dbo.Article", "Favorited");
            DropColumn("dbo.Article", "Commented");
            DropColumn("dbo.Article", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Article", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Article", "Commented", c => c.Boolean());
            AddColumn("dbo.Article", "Favorited", c => c.Boolean());
            AddColumn("dbo.Article", "Pointed", c => c.Boolean());
        }
    }
}
