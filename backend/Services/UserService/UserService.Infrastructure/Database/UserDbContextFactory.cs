using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.Infrastructure.Database;
public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=UserDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;");

        return new UserDbContext(optionsBuilder.Options);
    }
}