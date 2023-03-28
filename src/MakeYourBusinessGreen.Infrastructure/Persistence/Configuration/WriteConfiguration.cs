using MakeYourBusinessGreen.Domain.Entities;
using MakeYourBusinessGreen.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Configuration;
public class WriteConfiguration : IEntityTypeConfiguration<Office>, IEntityTypeConfiguration<Suggestion>, IEntityTypeConfiguration<StatusChangedEvent>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(id => id.Value, id => new OfficeId(id));

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(name => name.Value, name => new OfficeName(name));

        builder.ToTable("Offices");
    }

    public void Configure(EntityTypeBuilder<Suggestion> builder)
    {
        builder.HasKey(x => x.Id);

        builder
          .Property(x => x.Id)
          .HasConversion(id => id.Value, id => new SuggestionId(id));

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(title => title.Value, title => new SuggestionTitle(title));

        builder.Property(x => x.Body)
           .IsRequired()
           .HasMaxLength(200)
           .HasConversion(body => body.Value, body => new SuggestionBody(body));


        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(36)
            .HasConversion(id => id.Value, id => new UserId(id));

        builder.HasOne(x => x.Office);

        builder.HasMany(typeof(StatusChangedEvent), "_statusChangedEvents");

        builder.ToTable("Suggestions");
    }

    public void Configure(EntityTypeBuilder<StatusChangedEvent> builder)
    {
        builder.Property<Guid>("Id");

        builder.Property(x => x.ModeratorId)
            .IsRequired()
            .HasMaxLength(36)
            .HasConversion(id => id.Value, id => new UserId(id));

        builder.Property(x => x.Details)
            .IsRequired()
            .HasMaxLength(500);

        builder.ToTable("StatusChangeEvents");
    }
}
