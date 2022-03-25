using MakeYourBusinessGreen.Infastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries;
public class QueriesTestBase : IDisposable
{
    public ReadDbContext Context;
    public List<OfficeReadModel> Offices;
    public List<SuggestionReadModel> Suggestions;

    public QueriesTestBase()
    {
        var options = new DbContextOptionsBuilder<ReadDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

        Context = new ReadDbContext(options);

        Context.Database.EnsureCreated();

        Offices = new List<OfficeReadModel>()
        {
            new OfficeReadModel{Id = Guid.NewGuid(), Name="1"},
            new OfficeReadModel{Id = Guid.NewGuid(), Name="2"},
            new OfficeReadModel{Id = Guid.NewGuid(), Name="3"},
            new OfficeReadModel{Id = Guid.NewGuid(), Name="4"}
        };

        Suggestions = new List<SuggestionReadModel>
        {
           new SuggestionReadModel{Id = Guid.NewGuid(), Title="Title", Body="Body", Office=Offices[0], Status = Status.InProgress,
               Created = DateTime.UtcNow, UserId=Guid.NewGuid().ToString()},
            new SuggestionReadModel{Id = Guid.NewGuid(), Title="Title1", Body="Body1", Office=Offices[0], Status = Status.InProgress,
               Created = DateTime.UtcNow, UserId=Guid.NewGuid().ToString()},
             new SuggestionReadModel{Id = Guid.NewGuid(), Title="Title2", Body="Body2", Office=Offices[1], Status = Status.Archived,
               Created = DateTime.UtcNow, UserId=Guid.NewGuid().ToString()},
              new SuggestionReadModel{Id = Guid.NewGuid(), Title="Title3", Body="Body3", Office=Offices[2], Status = Status.Pending,
               Created = DateTime.UtcNow, UserId=Guid.NewGuid().ToString()},
        };

        Context.Offices.AddRange(Offices);
        Context.Suggestions.AddRange(Suggestions);

        Context.SaveChanges();

    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
