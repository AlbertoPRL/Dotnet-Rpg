using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data.Repositories;

public class CharacterRepository : Repository<Character>, ICharacterRepository
{
    private readonly DataContext _context;
    public CharacterRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Character>> FindAllCharactersAsync(int userId)
    {
        var characters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == userId)
            .ToListAsync();
            
        return characters;
    }

    public async Task<Character?> FindCharacterAsync(int characterId, int userId)
    {
        var character = await _context.Characters //Watch Include(c => c.User) function with a lambda exp to include the user
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == userId)
            .FirstOrDefaultAsync(c => c.Id == characterId);

        return character;
    }

    public async Task<Character?> FindIncludingSkillsAndWeaponsAsync(int characterId)
    {
        var character = await _context.Characters //Watch Include(c => c.User) function with a lambda exp to include the user
            .Include(c => c.Weapon)
            .Include(c => c.Skills)        
            .FirstOrDefaultAsync(c => c.Id == characterId);

        return character;
    }

     public async Task<Character?> FindIncludingSkillsAsync(int characterId)
    {
        var character = await _context.Characters //Watch Include(c => c.User) function with a lambda exp to include the user           
            .Include(c => c.Skills)        
            .FirstOrDefaultAsync(c => c.Id == characterId);

        return character;
    }

     public async Task<Character?> FindIncludingWeaponsAsync(int characterId)
    {
        var character = await _context.Characters //Watch Include(c => c.User) function with a lambda exp to include the user
            .Include(c => c.Weapon)        
            .FirstOrDefaultAsync(c => c.Id == characterId);

        return character;
    }

    public async Task<List<Character>?> FindAllByIdsAsync(List<int> characterIds)
    {
        var characters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => characterIds.Contains(c.Id)).ToListAsync();
        
        return characters;
    }

    public async Task<List<Character>?> FindAllWithAtLeastOneFight()
    {
       var characters = await _context.Characters
            .Where(c => c.Fights > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats)
            .ToListAsync();

        return characters;
    }   
}
