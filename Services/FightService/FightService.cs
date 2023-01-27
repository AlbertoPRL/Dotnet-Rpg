using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Data.Repositories;
using dotnet_rpg.Data.Repositories.Abstractions;
using dotnet_rpg.DTOs.Fight;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService;

public class FightService : IFightService
{
    private readonly ICharacterRepository _charRepo;
    private readonly IMapper _mapper;

    public FightService(ICharacterRepository charRepo, IMapper mapper)
    {
        _charRepo = charRepo;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
    {
        var response = new ServiceResponse<FightResultDto>
        {
            Data = new FightResultDto()
        };
        try
        {
            var characters = await _charRepo.FindAllByIdsAsync(request.CharacterIds);
            bool defeated = false;
            if (characters.Count > 1)
            {
                while (!defeated)
                {
                    foreach (Character attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;
                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        response.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");
                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} has been defeated");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left");
                            break;
                        }
                    }
                }
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });
                await _charRepo.SaveChangesAsync();
            }
            else
            {
                response.Message = "The Character can't fight alone";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;

    }

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
        {
            var attacker = await _charRepo.FindIncludingSkillsAsync(request.AttackerId);
            var opponent = await _charRepo.FindIncludingSkillsAsync(request.OpponentId);
            var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
            if (skill == null)
            {
                response.Message = "Skill not learned";
                return response;
            }
            int damage = DoSkillAttack(attacker, opponent, skill);
            if (opponent.HitPoints <= 0)
                response.Message = $"{opponent.Name} has been defeated";

            // response.Data = new AttackResultDto
            // {
            //     Attacker = attacker.Name,
            //     Opponent = opponent.Name,
            //     AttackerHp = attacker.HitPoints,
            //     OpponentHp = opponent.HitPoints,
            //     Damage = damage
            // };
            response.Data = _mapper.Map<AttackResultDto>(attacker);
            response.Data = _mapper.Map<AttackResultDto>(opponent);
            response.Data.Damage = damage;

            await _charRepo.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }

    private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
    {
        int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
        damage -= new Random().Next(opponent.Defense);

        if (damage > 0)
        {
            opponent.HitPoints -= damage;
        }

        return damage;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
        {
            var attacker = await _charRepo.FindIncludingWeaponsAsync(request.AttackerId);
            var opponent = await _charRepo.FindIncludingWeaponsAsync(request.OpponentId);
            int damage = DoWeponAttack(attacker, opponent);

            if (opponent.HitPoints <= 0)
                response.Message = $"{opponent.Name} has been defeated";

            await _charRepo.SaveChangesAsync();

            response.Data = _mapper.Map<AttackResultDto>(attacker);
            response.Data = _mapper.Map<AttackResultDto>(opponent);
            response.Data.Damage = damage;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }

    private static int DoWeponAttack(Character? attacker, Character? opponent)
    {
        int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
        damage -= new Random().Next(opponent.Defense);

        if (damage > 0)
        {
            opponent.HitPoints -= damage;
        }

        return damage;
    }

    public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
    {
        var characters = await _charRepo.FindAllWithAtLeastOneFight();
        var response = new ServiceResponse<List<HighScoreDto>>()
        {
            Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
        };
        return response;
    }
}
