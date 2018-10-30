namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageDSetIsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        LanguageInEnglish = c.String(),
                        LanguageInNative = c.String(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Language");
        }
    }
}
