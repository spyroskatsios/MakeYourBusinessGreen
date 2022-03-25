using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infastructure.Persistence.Interfaces;
public interface IIdentityDbContext
{
    DbSet<RefreshToken> RefreshTokens { get; }

    Task SaveAsync();
}
