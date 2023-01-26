namespace dotnet_rpg.Data.Repositories.Abstractions;

public interface IAuthRepository : IRepository<User>
{
    Task<User?> FindUserAsync(string username);
    Task<bool> UserExistAsync(string username);
}
