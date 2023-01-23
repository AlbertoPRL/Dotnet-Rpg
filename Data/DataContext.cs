using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data;

public class DataContext : DbContext
{
    public DbSet<Character> Characters { get; set; } //Usually you just need to prulalize the name of the entity.

    public DbSet<User> Users { get; set; } //This will create the conection with the database table "Users"

    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username })
            .IsUnique(true);
        modelBuilder.Entity<Skill>()
            .HasData(
                new Skill {Id = 1, Name = "Fireball", Damage = 30},
                new Skill {Id = 2, Name = "Frenzy", Damage = 20},
                new Skill {Id = 3, Name = "Blizzard", Damage = 50}
            );
    }
}
