using domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace db;

class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(user => user.Id);

        entity.Property(user => user.Name)
            .IsRequired();

        entity.Property(user => user.Email)
            .IsRequired();

        entity.HasIndex(user => user.Email)
            .IsUnique();

        entity.HasData(
            new User { Id = 1, Name = "Alice Johnson", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob Smith", Email = "bob@example.com" });
    }
}
