namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIdFromFavortiePoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "Pointed", c => c.Boolean());
            AddColumn("dbo.Article", "Favorited", c => c.Boolean());
            AddColumn("dbo.Article", "Commented", c => c.Boolean());
            AddColumn("dbo.Article", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "Discriminator");
            DropColumn("dbo.Article", "Commented");
            DropColumn("dbo.Article", "Favorited");
            DropColumn("dbo.Article", "Pointed");
        }
    }
}
