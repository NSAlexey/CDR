using CDR.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDR
{
    public class AppDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CallDb");
        }

        public DbSet<CallDetail> CallDetails { get; set; }
    }
}
