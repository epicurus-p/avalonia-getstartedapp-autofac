using Microsoft.EntityFrameworkCore;
using getstartedapp.Domain;

namespace getstartedapp.Infrastructure.Data;

public class AppDbContext(string connectionString) : DbContext
{
    public DbSet<TodoItem> Todos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(connectionString); // EF Core SQLite provider. 
    
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<TodoItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Priority).HasConversion<int>();
            e.Property(x => x.CreatedUtc).HasDefaultValueSql("CURRENT_TIMESTAMP");
            // indexes to speed up search/filter
            e.HasIndex(x => x.IsDone);
            e.HasIndex(x => x.DueDate);
            e.HasIndex(x => x.Priority);
        });
    }

}