
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Tournament> Tournaments { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;

    public static Task RunMigrationAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext.Database.MigrateAsync();
    }

    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!dbContext.Tournaments.Any())
        {
            var tournaments = new List<Tournament>();
            for (int i = 0; i < 20; i++)
            {
                tournaments.Add(Tournament.Create($"Tournament {i}").Value);
            }
            await dbContext.Tournaments.AddRangeAsync(tournaments);
            await dbContext.SaveChangesAsync();

            tournaments = await dbContext.Tournaments.ToListAsync();
            for (int i = 0; i < 4; i++)
            {
                var parentTournament = tournaments[i];

                parentTournament.AddSubTournament(tournaments[i + 1]);
            }

            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Players.Any())
        {
            var players = new List<Player>();
            for (int i = 0; i < 10; i++)
            {
                players.Add(Player.Create($"Player {i}", 20 + i).Value);
            }

            await dbContext.Players.AddRangeAsync(players);
            await dbContext.SaveChangesAsync();
        }
    }
}