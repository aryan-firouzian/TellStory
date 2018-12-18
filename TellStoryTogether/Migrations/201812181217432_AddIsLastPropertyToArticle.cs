namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsLastPropertyToArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "IsLast", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "IsLast");
        }
    }
}
