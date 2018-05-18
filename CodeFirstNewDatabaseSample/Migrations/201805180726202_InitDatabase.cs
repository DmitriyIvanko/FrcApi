namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Etalons",
                c => new
                    {
                        EtalonId = c.Guid(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ImageId = c.Guid(nullable: false),
                        FaceRecognitionSystemId = c.Guid(nullable: false),
                        FeatureMatrixId = c.Guid(nullable: false),
                        RegistredDT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EtalonId);
            
            CreateTable(
                "dbo.FaceRecognitionSystems",
                c => new
                    {
                        FaceRecognitionSystemId = c.Guid(nullable: false, identity: true),
                        MnemonicDescription = c.String(),
                        Type = c.String(),
                        TypeSystemId = c.Guid(nullable: false),
                        InputImageHeight = c.Int(nullable: false),
                        InputImageWidth = c.Int(nullable: false),
                        CreatedDT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FaceRecognitionSystemId);
            
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
            
            CreateTable(
                "dbo.LDAs",
                c => new
                    {
                        LDAId = c.Guid(nullable: false, identity: true),
                        AverageImageMatrixId = c.Guid(nullable: false),
                        LeftMatrixId = c.Guid(nullable: false),
                        RightMatrixId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.LDAId);
            
            CreateTable(
                "dbo.MatrixStrings",
                c => new
                    {
                        MatrixStringId = c.Guid(nullable: false, identity: true),
                        DimentionOne = c.Int(nullable: false),
                        DimentionTwo = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.MatrixStringId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "ImageDatabaseId", "dbo.ImageDatabases");
            DropIndex("dbo.Images", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "ImageDatabaseId" });
            DropTable("dbo.MatrixStrings");
            DropTable("dbo.LDAs");
            DropTable("dbo.Images");
            DropTable("dbo.Users");
            DropTable("dbo.ImageDatabases");
            DropTable("dbo.FaceRecognitionSystems");
            DropTable("dbo.Etalons");
        }
    }
}
