using MakeYourBusinessGreen.Domain.Entities;

namespace MakeYourBusinessGreen.Domain.Repositories;
public interface ISuggestionRepository
{
    Task<Suggestion?> GetAsync(SuggestionId id);
    Task AddAsync(Suggestion suggestion);
    Task DeleteAsync(Suggestion suggestion);
    Task UpdateAsync(Suggestion suggestion);
}
