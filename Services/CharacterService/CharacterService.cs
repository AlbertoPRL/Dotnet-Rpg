using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Data.Repositories;
using dotnet_rpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly CharacterRepository _charRepo;

    public CharacterService(IMapper mapper, DataContext context, CharacterRepository charRepo)
    {
        _context = context;
        _charRepo = charRepo;
        _mapper = mapper;
    }
       public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter, int userId)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();
        var character = _mapper.Map<Character>(newCharacter);
        character.UserId = userId;
        _charRepo.Add(character);
        await _charRepo.SaveChangesAsync();
        var characters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == userId)
            .ToListAsync();
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
        var characters = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == userId)
            .ToListAsync();
        response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id, int userId)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        var character = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User.Id == userId)
            .FirstOrDefaultAsync(c => c.Id == id);//(can also use FirstOrDefault();)
        response.Data = _mapper.Map<GetCharacterDto>(character);
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto characterToUpdate, int userId)
    {
        ServiceResponse<GetCharacterDto> response = new();
        try
        {
            var character = await _context.Characters //Watch Include(c => c.User) function with a lambda exp to include the user
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User.Id == userId)
                .FirstOrDefaultAsync(c => c.Id == characterToUpdate.Id);
            if (character == null)
            {
                response.Succes = false;
                response.Message = $"Character with id {characterToUpdate.Id} not found in database";
                return response;
            }
            _mapper.Map(characterToUpdate, character);
            _context.Characters.Update(character);
            _context.SaveChanges();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id, int userId)
    {
        ServiceResponse<List<GetCharacterDto>> response = new();
        try
        {
            var character = await _context.Characters
                .Where(c => c.User.Id == userId)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
            {
                response.Succes = false;
                response.Message = $"Character with id {id} not found in database";
                return response;
            }
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            var characters = await _context.Characters
                .Where(c => c.User.Id == userId)
                .ToListAsync();
            response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill, int userId)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                c.User.Id == userId);
            if(character == null)
            {
                response.Succes = false;
                response.Message = "Character not found";
                return response;
            }
            var skill = await _context.Skills.FindAsync(newCharacterSkill.SkillId);
            if(skill == null)
            {
                response.Succes = false;
                response.Message = "Skill not found";
                return response;
            }
            character.Skills.Add(skill);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch(Exception ex)
        {
            response.Succes = false;
            response.Message = ex.Message;            
        }
        return response;
    }
}
