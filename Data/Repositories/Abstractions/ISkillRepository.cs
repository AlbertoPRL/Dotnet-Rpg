using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Data.Repositories.Abstractions
{
    public interface ISkillRepository<Skill>
    {
        Task<Skill?> GetSkillAsync(int SkillId);
    }
}