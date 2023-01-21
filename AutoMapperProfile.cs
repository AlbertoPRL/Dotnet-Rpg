using AutoMapper;
using dotnet_rpg.DTOs.User;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<AddWeaponDto, Weapon>();
        }        
    }
}