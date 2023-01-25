using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Data.Repositories.Abstractions
{
    public interface ICharacterRepository<Character>
    {
        Task<List<Character>> GetAllCharactersAsync(int userId);
        Task<Character?> GetCharacterAsync(int characterId, int userId);   
        
    }
}