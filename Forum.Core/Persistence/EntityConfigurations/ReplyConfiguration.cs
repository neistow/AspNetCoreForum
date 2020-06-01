using Forum.Core.Concrete.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Core.Persistence.EntityConfigurations
{
    public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> entity)
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId).IsRequired();

            entity.HasOne(r => r.Author)
                .WithMany(u => u.Replies)
                .HasForeignKey(r => r.AuthorId).IsRequired();

            entity.Property(r => r.Text).HasMaxLength(1000).IsRequired();
            entity.Property(r => r.DateCreated).ValueGeneratedOnAdd();
        }
    }
}