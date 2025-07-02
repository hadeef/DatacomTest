// Ignore Spelling: Datacom


// Ignore Spelling: Datacom

using DatacomTest.Server.Models;

namespace DatacomTest.Server.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<ServiceResponse<Application>> AddAsync(Application application);
        Task<ServiceResponse<IEnumerable<Application>>> GetAllAsync(int? pageNumber = null, int? pageSize = null);
        Task<ServiceResponse<Application>> GetByIdAsync(int id);
        Task<ServiceResponse<Application>> UpdateAsync(Application application);
    }
}