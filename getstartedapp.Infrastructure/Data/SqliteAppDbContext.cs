using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace getstartedapp.Infrastructure.Data;

public class SqliteAppDbContext : AppDbContext
{
    public SqliteAppDbContext(DbContextOptions<SqliteAppDbContext> options)
        : base(options)
    {
    }
}


