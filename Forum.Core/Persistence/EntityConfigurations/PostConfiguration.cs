using Forum.Core.Concrete.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Core.Persistence.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> entity)
        {
            entity.HasKey(post => post.Id);

            entity.HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(p => p.Replies)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId).OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(p => p.AuthorId).IsRequired();
            
            entity.Property(post => post.Title).HasMaxLength(55).IsRequired();
            entity.Property(post => post.Text).HasMaxLength(5000).IsRequired();
            entity.Property(post => post.DateCreated).ValueGeneratedOnAdd();
        }
    }
}