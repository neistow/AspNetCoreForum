using Forum.Core.Concrete.Models;
using Forum.Core.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        // Add DbSet's here
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostTagConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}