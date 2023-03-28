using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Configuration;
public class ReadConfiguration : IEntityTypeConfiguration<SuggestionReadModel>, IEntityTypeConfiguration<OfficeReadModel>, IEntityTypeConfiguration<StatusChangedEventReadModel>
{
    public void Configure(EntityTypeBuilder<SuggestionReadModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(x => x.Body)
         .IsRequired()
         .HasMaxLength(200);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(36);

        builder.HasOne(x => x.Office)
            .WithMany();

        builder
            .HasMany(x => x.StatusChangedEvents)
            .WithOne(x => x.Suggestion);

        builder.ToTable("Suggestions");
    }

    public void Configure(EntityTypeBuilder<OfficeReadModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(50);

        builder.ToTable("Offices");
    }

    public void Configure(EntityTypeBuilder<StatusChangedEventReadModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ModeratorId)
           .IsRequired()
           .HasMaxLength(36);

        builder.Property(x => x.Details)
           .IsRequired()
           .HasMaxLength(500);

        builder.ToTable("StatusChangeEvents");
    }
}
