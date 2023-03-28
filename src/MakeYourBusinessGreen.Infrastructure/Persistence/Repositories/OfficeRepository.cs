using MakeYourBusinessGreen.Domain.Entities;
using MakeYourBusinessGreen.Domain.Repositories;
using MakeYourBusinessGreen.Domain.ValueObjects;
using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;

namespace MakeYourBusinessGreen.Infrastructure.Persistence.Repositories;
public class OfficeRepository : IOfficeRepository
{
    private readonly WriteDbContext _context;

    public OfficeRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Office office)
    {
        await _context.Offices.AddAsync(office);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Office office)
    {
        _context.Offices.Remove(office);
        await _context.SaveChangesAsync();
    }

    public async Task<Office?> GetAsync(OfficeId id)
    => await _context.Offices.FindAsync(id);

    public async Task UpdateAsync(Office office)
    {
        _context.Update(office);
        await _context.SaveChangesAsync();
    }
}
