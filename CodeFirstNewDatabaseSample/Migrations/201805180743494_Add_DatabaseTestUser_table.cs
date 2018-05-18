namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DatabaseTestUser_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatabaseTestUsers",
                c => new
                    {
                        DatabaseTestUserId = c.Guid(nullable: false, identity: true),
                        FaceRecognitionSystemId = c.Guid(nullable: false),
                        ImageId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DatabaseTestUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DatabaseTestUsers");
        }
    }
}
