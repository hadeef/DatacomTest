// Ignore Spelling: Datacom

// Ignore Spelling: Datacom

using DatacomTest.Server.Models;

namespace DatacomTest.Server.Repositories.Interfaces
{
    public interface IRepositoryApplications
    {
        Task<Application> AddAsync(Application application);

        Task<IEnumerable<Application>> GetAllAsync();

        IQueryable<Application> GetAllQueryable();

        Task<Application?> GetByIdAsync(int id);

        Task SaveChangesAsync();

        Task<Application> UpdateAsync(Application application);
    }
}