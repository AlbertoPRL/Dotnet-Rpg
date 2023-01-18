using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper, DataContext context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();
        var character = _mapper.Map<Character>(newCharacter);
        await _context.Characters.AddAsync(character);
        await _context.SaveChangesAsync();
        var characters = await _context.Characters.ToListAsync();
        response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return response;
    }
    
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();
        var characters = await _context.Characters.ToListAsync();
        response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        return response;

    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        var character = await _context.Characters.FindAsync(id);
        response.Data = _mapper.Map<GetCharacterDto>(character);
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto characterToUpdate)
    {
        ServiceResponse<GetCharacterDto> response = new();
        try
        {
            var character = await _context.Characters.FindAsync(characterToUpdate.Id);
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

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        ServiceResponse<List<GetCharacterDto>> response = new();
        try
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                response.Succes = false;
                response.Message = $"Character with id {id} not found in database";
                return response;
            }
            _context.Characters.Remove(character);
            _context.SaveChanges();
            response.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            response.Succes = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
