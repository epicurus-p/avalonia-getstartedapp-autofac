using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using getstartedapp.Domain;
using getstartedapp.Services;
using getstartedapp.Infrastructure.Data;

namespace getstartedapp.Infrastructure;

public class TodoService
{
    private readonly Func<AppDbContext> _dbFactory;

    public TodoService(Func<AppDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<List<TodoItem>> GetAllAsync()
    {
        using var db = _dbFactory();
        return await db.Todos.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(string title)
    {
        using var db = _dbFactory();
        db.Todos.Add(new TodoItem { Title = title, IsDone = false });
        await db.SaveChangesAsync();
    }

}