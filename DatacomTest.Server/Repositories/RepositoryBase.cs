// Ignore Spelling: Queryable

using DatacomTest.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatacomTest.Server.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly ILogger<RepositoryBase<TEntity>> _logger;

    public RepositoryBase(ApplicationDbContext context, ILogger<RepositoryBase<TEntity>> logger)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _logger = logger;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        _logger.LogInformation($"Adding a new {typeof(TEntity).Name} entity.");
        _ = _dbSet.Add(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        _logger.LogInformation($"Fetching all {typeof(TEntity).Name} entities.");
        List<TEntity> result = await _dbSet.ToListAsync();
        return result;
    }

    public IQueryable<TEntity> GetAllQueryable()
    {
        _logger.LogInformation($"Fetching all {typeof(TEntity).Name} entities.");
        IQueryable<TEntity> result = _dbSet.AsQueryable();
        return result;
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Fetching {typeof(TEntity).Name} entity with ID {id}.");
        TEntity? result = await _dbSet.FindAsync(id);
        return result;
    }

    public async Task SaveChangesAsync()
    {
        _ = await _context.SaveChangesAsync();
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _logger.LogInformation($"Updating {typeof(TEntity).Name} entity.");
        _ = _dbSet.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}