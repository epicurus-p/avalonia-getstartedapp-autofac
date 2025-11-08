using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace getstartedapp.Infrastructure.Data;

public class SqliteDesignTimeFactory : IDesignTimeDbContextFactory<SqliteAppDbContext>
{
    public SqliteAppDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "getstartedapp.Infrastructure");

        var cfg = ConfigurationLoader.Load();

        var connectionString = cfg.GetConnectionString("Sqlite");
        Console.WriteLine($"DesignTime BasePath = {basePath}");
        Console.WriteLine($"Sqlite ConnectionString = {(string.IsNullOrWhiteSpace(connectionString) ? "MISSING or EMPTY" : connectionString)}");

        var options = new DbContextOptionsBuilder<SqliteAppDbContext>()
            .UseSqlite(connectionString)
            .Options;

        return new SqliteAppDbContext(options);
    }
}


public class SqlServerDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerAppDbContext>
{
    public SqlServerAppDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "getstartedapp.Infrastructure");

        var cfg = ConfigurationLoader.Load();

        var connectionString = cfg.GetConnectionString("AzureSql");

        var options = new DbContextOptionsBuilder<SqlServerAppDbContext>()
            .UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            })
            .Options;


        return new SqlServerAppDbContext(options);
    }

}
