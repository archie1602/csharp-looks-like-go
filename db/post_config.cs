using domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace db;

class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> entity)
    {
        entity.HasKey(post => post.Id);

        entity.Property(post => post.Title)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(post => post.Body)
            .IsRequired();

        entity.Property(post => post.CreatedAt)
            .HasDefaultValueSql("now()");

        entity.HasOne(post => post.User)
            .WithMany(user => user.Posts)
            .HasForeignKey(post => post.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(post => post.UserId);
    }
}
