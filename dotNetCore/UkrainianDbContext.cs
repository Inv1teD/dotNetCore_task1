using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotNetMentorship.TestAPI
{

    public class UkrainianDbContext : DbContext
    {
        public DbSet<Ukrainian> Ukrainians { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here

            //Property Configurations
            modelBuilder.Entity<Ukrainian>()
                    .Property(u => u.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();
        }

        public UkrainianDbContext(DbContextOptions<UkrainianDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
    }
}
