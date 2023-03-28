using MakeYourBusinessGreen.Domain.Repositories;
using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Repositories;
public class RepositoryManager : IRepositoryManager
{
    private readonly WriteDbContext _context;

    private IOfficeRepository? _officeRepository;
    private ISuggestionRepository? _suggestionRepository;

    public RepositoryManager(WriteDbContext context)
    {
        _context = context;
    }

    public IOfficeRepository Office =>
            _officeRepository ?? new OfficeRepository(_context);

    public ISuggestionRepository Suggestion =>
        _suggestionRepository ?? new SuggestionRepository(_context);
}
