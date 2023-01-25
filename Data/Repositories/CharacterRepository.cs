using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data.Repositories;

public class CharacterRepository : Repository<Character>, ICharacterRepository<Character>
{
    private readonly DataContext _context;
    public CharacterRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Character>> GetAllCharactersAsync(int userId)
    {
        var characters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == userId)
            .ToListAsync();
        return characters;
    }

    public async Task<Character?> GetCharacterAsync(int characterId, int userId)
    {
        var character = await _context.Characters //Watch Include(c => c.User) function with a lambda exp to include the user
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User.Id == userId)
                .FirstOrDefaultAsync(c => c.Id == characterId);
        return character;
    }   
}
