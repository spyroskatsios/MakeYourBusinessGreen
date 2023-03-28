using MakeYourBusinessGreen.Domain.Entities;
using MakeYourBusinessGreen.Domain.Repositories;
using MakeYourBusinessGreen.Domain.ValueObjects;
using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Repositories;
public class SuggestionRepository : ISuggestionRepository
{
    private readonly WriteDbContext _context;

    public SuggestionRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Suggestion suggestion)
    {
        await _context.Suggestions.AddAsync(suggestion);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Suggestion suggestion)
    {
        _context.Suggestions.Remove(suggestion);
        await _context.SaveChangesAsync();
    }

    public async Task<Suggestion?> GetAsync(SuggestionId id)
    => await _context.Suggestions.Include(x => x.Office).Include("_statusChangedEvents").FirstOrDefaultAsync(x => x.Id == id);

    public async Task UpdateAsync(Suggestion suggestion)
    {
        _context.Suggestions.Update(suggestion);
        await _context.SaveChangesAsync();
    }
}
