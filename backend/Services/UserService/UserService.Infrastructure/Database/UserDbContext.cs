using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Database.Entities;

namespace UserService.Infrastructure.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<UserTeamRole> UserTeamRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTeamRole>().HasKey(utr => new { utr.UserId, utr.TeamId });

        modelBuilder.Entity<UserTeamRole>()
            .HasOne(utr => utr.User)
            .WithMany(u => u.UserTeamRoles)
            .HasForeignKey(utr => utr.UserId);

        modelBuilder.Entity<UserTeamRole>()
            .HasOne(utr => utr.Team)
            .WithMany(t => t.UserTeamRoles)
            .HasForeignKey(utr => utr.TeamId);

        base.OnModelCreating(modelBuilder);
    }
}
