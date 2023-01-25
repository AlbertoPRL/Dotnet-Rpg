using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data.Repositories;

public class CharacterRepository : Repository<Character>, ICharacterRepository<Character>
{
        private readonly DbContext _context;
    public CharacterRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    
}
