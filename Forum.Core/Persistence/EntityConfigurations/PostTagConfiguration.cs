using Forum.Core.Concrete.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Core.Persistence.EntityConfigurations
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> entity)
        {
            entity.HasKey(postTag => new {postTag.TagId, postTag.PostId});

            entity.HasOne(postTag => postTag.Post)
                .WithMany(post => post.PostTags)
                .HasForeignKey(postTag => postTag.PostId);

            entity.HasOne(postTag => postTag.Tag)
                .WithMany(tag => tag.PostTags)
                .HasForeignKey(postTag => postTag.TagId);
        }
    }
}