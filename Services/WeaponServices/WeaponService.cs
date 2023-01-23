using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponServices
{
    public class WeaponService : IWeaponService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _context;
        public WeaponService(DataContext context, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon, int userId)
        {
            ServiceResponse<GetCharacterDto> response = new();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Skills)
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                    c.User.Id == userId);//Ask Marcel about implementation of this GetUserId() Method
                if(character == null)
                {
                    response.Succes = false;
                    response.Message = "Character not found";
                    return response;
                }
                var weapon = _mapper.Map<Weapon>(newWeapon);
                _context.Weapons.Add(weapon);
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
}