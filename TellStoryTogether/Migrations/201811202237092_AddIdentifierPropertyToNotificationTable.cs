namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentifierPropertyToNotificationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Identifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Identifier");
        }
    }
}
