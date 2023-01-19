using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Character> Characters { get; set; } //Usually you just need to prulalize the name of the entity.
        public DbSet<User>  Users {get; set;} //This will create the conexion with the database table "Users"
    }
}