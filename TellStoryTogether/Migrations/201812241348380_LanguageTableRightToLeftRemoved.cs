namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageTableRightToLeftRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Language", "RightToLeft");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Language", "RightToLeft", c => c.Boolean(nullable: false));
        }
    }
}
