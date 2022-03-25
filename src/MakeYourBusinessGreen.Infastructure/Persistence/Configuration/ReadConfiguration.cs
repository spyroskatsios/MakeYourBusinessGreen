using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeYourBusinessGreen.Infastructure.Persistence.Configuration;
public class ReadConfiguration : IEntityTypeConfiguration<SuggestionReadModel>, IEntityTypeConfiguration<OfficeReadModel>, IEntityTypeConfiguration<StatusChangedEventReadModel>
{
    public void Configure(EntityTypeBuilder<SuggestionReadModel> builder)
    {
        builder.HasKey(x => x.Id);

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
        builder.ToTable("Offices");
    }

    public void Configure(EntityTypeBuilder<StatusChangedEventReadModel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("StatusChangeEvents");
    }
}
