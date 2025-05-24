using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskService.Infrastructure.Database;
public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TaskDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;");

        return new TaskDbContext(optionsBuilder.Options);
    }
}