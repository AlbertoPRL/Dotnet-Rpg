using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Data.Repositories;
using dotnet_rpg.Data.Repositories.Abstractions;
using dotnet_rpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly ICharacterRepository _charRepo;
    private readonly IRepository<Skill> _skillRepo;

    public CharacterService(IMapper mapper, ICharacterRepository charRepo, IRepository<Skill> skillRepo)
    {
        _charRepo = charRepo;
        _skillRepo = skillRepo;
        _mapper = mapper;
    }
       public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter, int userId)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();
        var character = _mapper.Map<Character>(newCharacter);
        character.UserId = userId;
        _charRepo.Add(character); 
        await _charRepo.SaveChangesAsync();              
        var characters = await _charRepo.FindAllCharactersAsync(userId);
        response.Data = characters            
            .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return response;
        // response.Data = await _context.Characters
        //     .Select(c => _mapper.Map<GetCharacterDto>(c))
        //     .ToListAsync();
    }
    
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();
        var characters = await _charRepo.FindAllCharactersAsync(userId);
        response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id, int userId)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        var character = await GetCharacterById(id, userId);
        response.Data = _mapper.Map<GetCharacterDto>(character);
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto characterToUpdate, int userId)
    {
        ServiceResponse<GetCharacterDto> response = new();
        try
        {
            var character = await _charRepo.FindCharacterAsync(characterToUpdate.Id, userId);
            if (character == null)
            {
                response.Message = $"Character with id {characterToUpdate.Id} not found in database";
                return response;
            }
            _mapper.Map(characterToUpdate, character);
            _charRepo.Update(character);
            await _charRepo.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int characterId, int userId)
    {
        ServiceResponse<List<GetCharacterDto>> response = new();
        try
        {
            var character = await _charRepo.FindCharacterAsync(characterId, userId);
            if (character == null)
            {
                response.Message = $"Character with id {characterId} not found in database";
                return response;
            }
            _charRepo.Remove(character);
            await _charRepo.SaveChangesAsync();
            var characters = await _charRepo.FindAllCharactersAsync(userId);
            response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill, int userId)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _charRepo.FindCharacterAsync(newCharacterSkill.CharacterId, userId);
                //_context.Characters
                //.Include(c => c.Weapon)
                //.Include(c => c.Skills)
                //.FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                //c.User.Id == userId);
            if(character == null)
            {
                response.Message = "Character not found";
                return response;
            }
            var skill = await _skillRepo.FindAsync(newCharacterSkill.SkillId);
            if(skill == null)
            {
                response.Message = "Skill not found";
                return response;
            }
            character.Skills.Add(skill);
            _charRepo.Update(character); //Dudas
            await _charRepo.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch(Exception ex)
        {
            response.Message = ex.Message;            
        }
        return response;
    }
}
