namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageTableTextDirectionAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Language", "TextDirection", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Language", "TextDirection");
        }
    }
}
