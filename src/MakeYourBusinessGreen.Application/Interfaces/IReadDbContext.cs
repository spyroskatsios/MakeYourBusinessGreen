using MakeYourBusinessGreen.Application.Models;

namespace MakeYourBusinessGreen.Application.Interfaces;
public interface IReadDbContext
{
    DbSet<OfficeReadModel> Offices { get; }
    DbSet<SuggestionReadModel> Suggestions { get; }

}
