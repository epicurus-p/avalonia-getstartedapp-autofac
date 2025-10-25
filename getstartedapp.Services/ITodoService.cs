using getstartedapp.Domain;

namespace getstartedapp.Services;

public enum TodoFilter { All, Active, Completed, DueToday, Overdue }

public interface ITodoService
{
    Task<List<TodoItem>> GetAllAsync();
   
    Task<IReadOnlyList<TodoItem>> QueryAsync(
        TodoFilter filter,
        string? search,
        CancellationToken ct = default);
    
    Task<TodoItem> AddAsync(string title, TodoPriority priority, DateTime? dueDate, string? notes, CancellationToken ct = default);
    Task ToggleDoneAsync(int id, bool isDone, CancellationToken ct = default);
    Task UpdateAsync(TodoItem updated, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

}