using System.Data.Entity;
using Data.Entities;

namespace Data
{
    class FrcContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageDatabase> ImageDatabases { get; set; }

        public FrcContext()
            : base("name=FrcContextDatabase")
        {
        }
    }
}
