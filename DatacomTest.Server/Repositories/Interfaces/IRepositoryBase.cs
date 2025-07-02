namespace DatacomTest.Server.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> GetAllQueryable();

        Task<TEntity?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}