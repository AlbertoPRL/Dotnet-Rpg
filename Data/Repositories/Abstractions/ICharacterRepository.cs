namespace dotnet_rpg.Data.Repositories.Abstractions;

public interface ICharacterRepository : IRepository<Character>
{
    Task<List<Character>> FindAllCharactersAsync(int userId);
    Task<Character?> FindCharacterAsync(int characterId, int userId);
    Task<List<Character>> FindAllByIdsAsync(List<int> characterIds);
    Task<Character?> FindIncludingSkillsAndWeaponsAsync(int characterId);
    Task<Character?> FindIncludingSkillsAsync(int characterId);
    Task<Character?> FindIncludingWeaponsAsync(int characterId);
    Task<List<Character>?> FindAllWithAtLeastOneFight();
}
