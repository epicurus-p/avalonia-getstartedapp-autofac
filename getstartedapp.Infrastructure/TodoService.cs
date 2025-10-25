using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using getstartedapp.Domain;
using getstartedapp.Services;
using getstartedapp.Infrastructure.Data;

namespace getstartedapp.Infrastructure;

public class TodoService : ITodoService
{
    private readonly Func<AppDbContext> _dbFactory;

    public TodoService(Func<AppDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<List<TodoItem>> GetAllAsync()
    {
        using var db = _dbFactory();
        return await db.Todos.AsNoTracking().ToListAsync();
    }
    
    public async Task<IReadOnlyList<TodoItem>> QueryAsync(TodoFilter filter, string? search, CancellationToken ct = default)
    {
        using var db = _dbFactory();
        var q = db.Todos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            q = q.Where(t => EF.Functions.Like(t.Title, $"%{term}%") || (t.Notes != null && EF.Functions.Like(t.Notes, $"%{term}%")));
        }

        var today = DateTime.UtcNow.Date;
        q = filter switch
        {
            TodoFilter.Active    => q.Where(t => !t.IsDone),
            TodoFilter.Completed => q.Where(t =>  t.IsDone),
            TodoFilter.DueToday  => q.Where(t => !t.IsDone && t.DueDate != null && t.DueDate.Value.Date <= today),
            TodoFilter.Overdue   => q.Where(t => !t.IsDone && t.DueDate != null && t.DueDate.Value.Date <  today),
            _ => q
        };

        return await q.OrderBy(t => t.IsDone)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .ThenBy(t => t.Id)
            .ToListAsync(ct);
    }
   
    public async Task<TodoItem> AddAsync(string title, TodoPriority priority, DateTime? dueDate, string? notes, CancellationToken ct = default)
    { 
        using var db = _dbFactory();
        var item = new TodoItem { Title = title, Priority = priority, DueDate = dueDate, Notes = notes, IsDone = false, CreatedUtc = DateTime.UtcNow };
        db.Todos.Add(item); 
        await db.SaveChangesAsync(ct);
        return item;
    }

    public async Task ToggleDoneAsync(int id, bool isDone, CancellationToken ct = default)
    {
        using var db = _dbFactory();
        var item = await db.Todos.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (item is null) return;
        item.IsDone   = isDone;
        item.UpdatedUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TodoItem updated, CancellationToken ct = default)
    {
        using var db = _dbFactory();
        updated.UpdatedUtc = DateTime.UtcNow;
        db.Todos.Update(updated);
        await db.SaveChangesAsync(ct);
    }
    
    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        using var db = _dbFactory();
        var item = new TodoItem { Id = id };
        db.Entry(item).State = EntityState.Deleted;
        await db.SaveChangesAsync(ct);
    }

}