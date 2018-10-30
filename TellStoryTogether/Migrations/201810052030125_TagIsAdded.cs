namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagIsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Detail = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tag");
        }
    }
}
