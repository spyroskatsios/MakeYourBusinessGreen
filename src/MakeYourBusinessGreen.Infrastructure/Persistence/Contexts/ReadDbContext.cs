using MakeYourBusinessGreen.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
public class ReadDbContext : DbContext, IReadDbContext
{
    public DbSet<OfficeReadModel> Offices { get; set; }
    public DbSet<SuggestionReadModel> Suggestions { get; set; }


    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("MakeYourBusinessGreen");

        var configuration = new ReadConfiguration();

        builder.ApplyConfiguration<OfficeReadModel>(configuration);
        builder.ApplyConfiguration<SuggestionReadModel>(configuration);
        builder.ApplyConfiguration<StatusChangedEventReadModel>(configuration);
        base.OnModelCreating(builder);
    }

    public async Task SaveAsync()
    {
        await SaveChangesAsync();
    }
}
