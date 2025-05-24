using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.Infrastructure.Database;
public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var connectionString = args.Length > 0 
            ? args[0] 
            : null;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found. Please provide it either through args ");
        }
            
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseSqlServer(connectionString); // "Server=localhost,1433;Database=UserDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"

        return new UserDbContext(optionsBuilder.Options);
    }
}