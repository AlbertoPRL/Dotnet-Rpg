using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_rpg.Controllers
{
    [ApiController] //Attributes 
    [Route("api/[controller]")]
    
    public class CharacterController : ControllerBase //To be a propper controller the class has to inherit from ControllerBase(take a look after to the class)

    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;            
        }
        
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()//Look for IActionResultClass
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]//THE PARAMETER ROUTE SHOULD MATCH THE PARAMATER NAME ON THE FUNCTION ITSELF AND SHOULD BE BETWEEN CURLY BRACES
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> addCharacter(AddCharacterDto newCharacter)
        {            
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if(response.Data == null)
            {
                return NotFound(response);
            }            
            return Ok(response);
        }
    }
}