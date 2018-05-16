namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ImageDatabase_User_Image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageDatabases",
                c => new
                    {
                        ImageDatabaseId = c.Guid(nullable: false, identity: true),
                        DatabaseName = c.String(),
                        ImageHeight = c.Int(nullable: false),
                        ImageWidth = c.Int(nullable: false),
                        TotalUser = c.Int(nullable: false),
                        TotalImageForUser = c.Int(nullable: false),
                        isSameTotalImageForUser = c.Boolean(nullable: false),
                        isSameImageSize = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImageDatabaseId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        Username = c.String(),
                        ImageDatabaseId = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ImageDatabases", t => t.ImageDatabaseId)
                .Index(t => t.ImageDatabaseId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Guid(nullable: false, identity: true),
                        ImageByteArray = c.Binary(),
                        UserId = c.Guid(nullable: false),
                        Format = c.String(),
                        ImageName = c.String(),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "ImageDatabaseId", "dbo.ImageDatabases");
            DropIndex("dbo.Images", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "ImageDatabaseId" });
            DropTable("dbo.Images");
            DropTable("dbo.Users");
            DropTable("dbo.ImageDatabases");
        }
    }
}
