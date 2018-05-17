namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Matrix_string : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatrixStrings",
                c => new
                    {
                        MatrixStringId = c.Guid(nullable: false, identity: true),
                        DimentionOne = c.Int(nullable: false),
                        DimentionTwo = c.Int(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.MatrixStringId);
            
            DropTable("dbo.Matrices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Matrices",
                c => new
                    {
                        MatrixId = c.Guid(nullable: false, identity: true),
                        DimentionOne = c.Int(nullable: false),
                        DimentionTwo = c.Int(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.MatrixId);
            
            DropTable("dbo.MatrixStrings");
        }
    }
}
