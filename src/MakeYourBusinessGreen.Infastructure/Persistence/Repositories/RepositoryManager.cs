using MakeYourBusinessGreen.Domain.Repositories;

namespace MakeYourBusinessGreen.Infastructure.Persistence.Repositories;
public class RepositoryManager : IRepositoryManager
{
    private readonly WriteDbContext _context;

    private IOfficeRepository _officeRepository;
    private ISuggestionRepository _suggestionRepository;

    public RepositoryManager(WriteDbContext context)
    {
        _context = context;
    }

    public IOfficeRepository Office =>
            _officeRepository is null ? new OfficeRepository(_context) : _officeRepository;

    public ISuggestionRepository Suggestion =>
        _suggestionRepository is null ? new SuggestionRepository(_context) : _suggestionRepository;
}
