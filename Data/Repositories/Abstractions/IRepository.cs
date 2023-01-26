namespace dotnet_rpg.Data.Repositories.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Add(TEntity entity);
    Task<TEntity?> FindAsync(int id);
    Task<IEnumerable<TEntity>> FindAllAsync();
    void Remove(TEntity entity);
    TEntity Update(TEntity entity);
    Task<bool> SaveChangesAsync();
}
