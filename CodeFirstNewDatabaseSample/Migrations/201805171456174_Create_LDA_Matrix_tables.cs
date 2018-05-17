namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_LDA_Matrix_tables : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Matrices",
                c => new
                    {
                        MatrixId = c.Guid(nullable: false, identity: true),
                        DimentionOne = c.Int(nullable: false),
                        DimentionTwo = c.Int(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.MatrixId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Matrices");
            DropTable("dbo.LDAs");
        }
    }
}
