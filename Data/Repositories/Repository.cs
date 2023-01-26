using dotnet_rpg.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context;

    public Repository(DataContext context)
    {
        _context = context;
    }

    public TEntity Add(TEntity entity)
    {
        return _context.Add(entity).Entity;
    }

    public async Task<TEntity?> FindAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public TEntity Update(TEntity entity)
    {
        return _context.Set<TEntity>().Update(entity).Entity;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
