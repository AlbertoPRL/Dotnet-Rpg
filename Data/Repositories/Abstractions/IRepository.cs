namespace dotnet_rpg.Data.Repositories.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Add(TEntity entity);
    Task<TEntity?> Find(int id);
    IEnumerable<TEntity> FindAll();
    void Remove(TEntity entity);
    TEntity Update(TEntity entity);
    Task<bool> SaveChangesAsync();
}
