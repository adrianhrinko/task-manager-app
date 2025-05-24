using Microsoft.EntityFrameworkCore;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure
{
    public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Project> Projects { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.TaskItems)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedTo);
        }
    }
}
