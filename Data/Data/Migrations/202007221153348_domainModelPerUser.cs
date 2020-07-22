namespace ReadLater.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class domainModelPerUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "UserId", c => c.String());
            AddColumn("dbo.Bookmarks", "ClickedCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Bookmarks", "URL", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookmarks", "URL", c => c.String(maxLength: 500));
            DropColumn("dbo.Bookmarks", "ClickedCount");
            DropColumn("dbo.Categories", "UserId");
        }
    }
}
