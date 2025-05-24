using Microsoft.EntityFrameworkCore;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure.Database;

public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Epic> Epics { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Project - TaskItem relationship (Tasks deleted with Project)
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Project)
            .WithMany(p => p.TaskItems)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);  // NoAction because tasks will be deleted through Epic

        // Project - Epic relationship (Epics deleted with Project)
        modelBuilder.Entity<Epic>()
            .HasOne(e => e.Project)
            .WithMany(p => p.Epics)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Epic - TaskItem relationship (Tasks deleted with Epic)
        modelBuilder.Entity<TaskItem>()
            .HasOne<Epic>()
            .WithMany(e => e.Tasks)
            .HasForeignKey(t => t.EpicId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - TaskItem (Assignment) relationship
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.AssignedTo)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.NoAction);

        // Project - Owner (User) relationship (Projects deleted with owner)
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project - Team relationship (Projects deleted with team)
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Projects)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
