using getstartedapp.Domain;

namespace getstartedapp.Services;

public interface ITodoService
{
    public Task<List<TodoItem>> GetAllAsync();

    public Task AddAsync(string title);
}