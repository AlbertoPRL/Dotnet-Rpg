using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data.Repositories.Abstractions;

namespace dotnet_rpg.Data.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository<Skill>
{
    private readonly DataContext _context;

    public SkillRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Skill?> GetSkillAsync(int SkillId)
    {
        var skill = await _context.Skills.FindAsync(SkillId);
        return skill;
    }
}
