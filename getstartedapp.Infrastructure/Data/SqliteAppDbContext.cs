using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace getstartedapp.Infrastructure.Data;

public class SqliteAppDbContext : AppDbContext
{
    private readonly IConfiguration? _configuration;

    public SqliteAppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqliteAppDbContext(DbContextOptions<SqliteAppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var cs = _configuration?.GetConnectionString("Sqlite")
                     ?? "Data Source=./data/dev.db";
            optionsBuilder.UseSqlite(cs);
        }
    }
}
