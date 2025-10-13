using Microsoft.EntityFrameworkCore;
using getstartedapp.Domain;

namespace getstartedapp.Infrastructure.Data;

public class AppDbContext(string connectionString) : DbContext
{
    public DbSet<TodoItem> Todos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(connectionString); // EF Core SQLite provider. 
}