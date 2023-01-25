using dotnet_rpg.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;

    public Repository(DbContext context)
    {
        _context = context;
    }

    public TEntity Add(TEntity entity)
    {
        return _context.Add(entity).Entity;
    }

    public async Task<TEntity?> Find(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public IEnumerable<TEntity> FindAll()
    {
        return _context.Set<TEntity>().ToList();
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
