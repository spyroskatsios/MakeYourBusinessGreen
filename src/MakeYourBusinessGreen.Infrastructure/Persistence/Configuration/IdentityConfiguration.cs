using MakeYourBusinessGreen.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Configuration;
public class IdentityConfiguration : IEntityTypeConfiguration<User>, IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder
           .Property(x => x.LastName)
           .IsRequired()
           .HasMaxLength(50);

    }
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.
               HasKey(x => x.Id);

        builder.
            Property(x => x.Value)
            .IsRequired();

        builder
           .Property(x => x.UserId)
           .IsRequired()
           .HasMaxLength(36);
    }

}
