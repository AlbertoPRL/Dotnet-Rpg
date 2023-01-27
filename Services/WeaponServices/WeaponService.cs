using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Data.Repositories.Abstractions;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponServices;

public class WeaponService : IWeaponService
{
    private readonly IRepository<Weapon> _weaponRepo;
    private readonly IMapper _mapper;
    private readonly ICharacterRepository _charRepo;
    
    public WeaponService(IRepository<Weapon> weaponRepo, IMapper mapper, ICharacterRepository charRepo)
    {
        _weaponRepo = weaponRepo;
        _mapper = mapper;
        _charRepo = charRepo;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon, int userId)
    {
        ServiceResponse<GetCharacterDto> response = new();
        try
        {
            var character = await _charRepo.FindCharacterAsync(newWeapon.CharacterId, userId);
            if (character == null)
            {
                response.Message = "Character not found";
                return response;
            }
            var weapon = _mapper.Map<Weapon>(newWeapon);
            _weaponRepo.Add(weapon);
            await _weaponRepo.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }
}
