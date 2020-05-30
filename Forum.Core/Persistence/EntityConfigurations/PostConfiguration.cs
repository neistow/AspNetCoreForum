using Forum.Core.Concrete.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Core.Persistence.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(post => post.Id);

            builder.Property(post => post.Title).HasMaxLength(55).IsRequired();
            builder.Property(post => post.Text).HasMaxLength(5000).IsRequired();
            builder.Property(post => post.DateCreated).ValueGeneratedOnAdd();
        }
    }
}