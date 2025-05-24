using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskService.Infrastructure.Database;
public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        var connectionString = args.Length > 0 
            ? args[0] 
            : null;
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found. Please provide it either through args");
        }

        var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
        optionsBuilder.UseSqlServer(connectionString); // "Server=localhost,1433;Database=TaskDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"

        return new TaskDbContext(optionsBuilder.Options);
    }
}