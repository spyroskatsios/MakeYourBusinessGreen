using MakeYourBusinessGreen.Domain.Entities;

namespace MakeYourBusinessGreen.Domain.Repositories
{
    public interface IOfficeRepository
    {
        Task<Office?> GetAsync(OfficeId id);
        Task AddAsync(Office office);
        Task DeleteAsync(Office office);
        Task UpdateAsync(Office office);
    }
}
