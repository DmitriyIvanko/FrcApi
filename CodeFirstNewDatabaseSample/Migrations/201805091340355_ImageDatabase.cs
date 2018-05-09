namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageDatabases",
                c => new
                    {
                        ImageDatabaseId = c.Guid(nullable: false, identity: true),
                        DatabaseName = c.String(),
                    })
                .PrimaryKey(t => t.ImageDatabaseId);
            
            AddColumn("dbo.Photos", "Format", c => c.String());
            AddColumn("dbo.Users", "ImageDatabaseId", c => c.Guid());
            CreateIndex("dbo.Users", "ImageDatabaseId");
            AddForeignKey("dbo.Users", "ImageDatabaseId", "dbo.ImageDatabases", "ImageDatabaseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ImageDatabaseId", "dbo.ImageDatabases");
            DropIndex("dbo.Users", new[] { "ImageDatabaseId" });
            DropColumn("dbo.Users", "ImageDatabaseId");
            DropColumn("dbo.Photos", "Format");
            DropTable("dbo.ImageDatabases");
        }
    }
}
