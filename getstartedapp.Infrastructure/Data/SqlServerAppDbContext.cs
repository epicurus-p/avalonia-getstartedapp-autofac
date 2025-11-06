using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace getstartedapp.Infrastructure.Data;


public class SqlServerAppDbContext : AppDbContext
{
    private readonly IConfiguration? _configuration;

    public SqlServerAppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlServerAppDbContext(DbContextOptions<SqlServerAppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var cs = _configuration?.GetConnectionString("AzureSql")
                     ?? throw new InvalidOperationException("AzureSql connection string missing.");
            optionsBuilder.UseSqlServer(cs);
        }
    }
}
