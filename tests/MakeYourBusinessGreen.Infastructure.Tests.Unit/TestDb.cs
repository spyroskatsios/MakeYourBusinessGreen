using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace MakeYourBusinessGreen.Infastructure.Tests.Unit;
public class TestDb : IDisposable
{
    public IdentityDbContext Context;

    public TestDb()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

        Context = new IdentityDbContext(options);

        Context.Database.EnsureCreated();

    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
