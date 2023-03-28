using MakeYourBusinessGreen.Infrastructure.Authentication;
using MakeYourBusinessGreen.Infrastructure.Persistence.Configuration;
using MakeYourBusinessGreen.Infrastructure.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
public class IdentityDbContext : IdentityDbContext<User>, IIdentityDbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("MakeYourBusinessGreen");

        var configuration = new IdentityConfiguration();

        builder.ApplyConfiguration<RefreshToken>(configuration);
        builder.ApplyConfiguration<User>(configuration);
        base.OnModelCreating(builder);
    }

    public async Task SaveAsync()
        => await SaveChangesAsync();
}
