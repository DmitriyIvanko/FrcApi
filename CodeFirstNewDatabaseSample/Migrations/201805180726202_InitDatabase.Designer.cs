// <auto-generated />
namespace CodeFirstNewDatabaseSample.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class InitDatabase : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(InitDatabase));
        
        string IMigrationMetadata.Id
        {
            get { return "201805180726202_InitDatabase"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}