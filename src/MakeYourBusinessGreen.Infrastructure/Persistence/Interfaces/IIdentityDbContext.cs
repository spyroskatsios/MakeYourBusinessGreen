using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Interfaces;
public interface IIdentityDbContext
{
    DbSet<RefreshToken> RefreshTokens { get; }

    Task SaveAsync();
}
