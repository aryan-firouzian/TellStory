namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RightToLeft_toLanguageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Language", "RightToLeft", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Language", "RightToLeft");
        }
    }
}
