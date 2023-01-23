using dotnet_rpg.Controllers.Abstractions;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[Authorize]
[ApiController] //Attributes 
[Route("api/[controller]")]
public class CharacterController : UserContextController //To be a propper controller the class has to inherit from ControllerBase(take a look after to the class)

{
    private readonly ICharacterService _characterService;
    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()//Look for IActionResultClass
    {
        return Ok(await _characterService.GetAllCharacters(GetUserId()));
    }

    [HttpGet("{id}")]//THE PARAMETER ROUTE SHOULD MATCH THE PARAMATER NAME ON THE FUNCTION ITSELF AND SHOULD BE BETWEEN CURLY BRACES
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        return Ok(await _characterService.GetCharacterById(id, GetUserId()));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> addCharacter(AddCharacterDto newCharacter)
    {
        return Ok(await _characterService.AddCharacter(newCharacter, GetUserId()));
    }
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter, GetUserId());
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
        var response = await _characterService.DeleteCharacter(id, GetUserId());
        if (response.Data == null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newSkill)
    {
        return Ok(await _characterService.AddCharacterSkill(newSkill, GetUserId()));
    }
}
