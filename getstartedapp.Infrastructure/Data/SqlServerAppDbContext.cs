using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace getstartedapp.Infrastructure.Data;


public class SqlServerAppDbContext : AppDbContext
{
    public SqlServerAppDbContext(DbContextOptions<SqlServerAppDbContext> options)
        : base(options)
    {
    }
}

