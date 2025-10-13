using Microsoft.EntityFrameworkCore.Design;

namespace getstartedapp.Infrastructure.Data;

public class DesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        return new AppDbContext(DbConfig.GetConnectionString()); 
    }
}
