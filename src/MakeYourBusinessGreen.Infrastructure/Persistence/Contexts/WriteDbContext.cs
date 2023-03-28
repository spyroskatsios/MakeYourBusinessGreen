using MakeYourBusinessGreen.Domain.Entities;
using MakeYourBusinessGreen.Domain.ValueObjects;
using MakeYourBusinessGreen.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
public class WriteDbContext : DbContext
{
    public DbSet<Office> Offices { get; set; }
    public DbSet<Suggestion> Suggestions { get; set; }

    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("MakeYourBusinessGreen");

        var configuration = new WriteConfiguration();

        builder.ApplyConfiguration<Office>(configuration);
        builder.ApplyConfiguration<Suggestion>(configuration);
        builder.ApplyConfiguration<StatusChangedEvent>(configuration);
        base.OnModelCreating(builder);
    }

}
