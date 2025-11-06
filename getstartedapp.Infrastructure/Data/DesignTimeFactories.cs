using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace getstartedapp.Infrastructure.Data;

public class SqliteDesignTimeFactory : IDesignTimeDbContextFactory<SqliteAppDbContext>
{
    public SqliteAppDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<SqliteAppDbContext>()
            .UseSqlite(cfg.GetConnectionString("Sqlite"))
            .Options;

        // Supply IConfiguration via ctor
        return new SqliteAppDbContext(options);
    }
}

public class SqlServerDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerAppDbContext>
{
    public SqlServerAppDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<SqlServerAppDbContext>()
            .UseSqlServer(cfg.GetConnectionString("AzureSql"))
            .Options;
        
        
        Console.WriteLine($"DesignTime BasePath = {Directory.GetCurrentDirectory()}");
        Console.WriteLine($"AzureSql = {(cfg?.Length ?? 0) > 0 ? "present" : "MISSING"}");


        return new SqlServerAppDbContext(options);
    }
}
