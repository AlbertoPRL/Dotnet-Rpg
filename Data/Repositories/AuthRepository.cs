using dotnet_rpg.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data.Repositories;

public class AuthRepository : Repository<User>, IAuthRepository
{
    private readonly DataContext _context;

    public AuthRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> FindUserAsync(string username)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public async Task<bool> UserExistAsync(string username)
    {
        var user = await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        return user;
    }

}
