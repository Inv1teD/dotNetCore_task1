using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotNetMentorship.TestAPI
{

    public class UkrainianDbContext : DbContext
    {
        public DbSet<Ukrainian> Ukrainians { get; set; }

        public UkrainianDbContext(DbContextOptions<UkrainianDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
    }
}
