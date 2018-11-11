namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeToComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comment", "Time");
        }
    }
}
