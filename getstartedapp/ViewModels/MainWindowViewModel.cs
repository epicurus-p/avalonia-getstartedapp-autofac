using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using getstartedapp.Infrastructure;
using getstartedapp.Infrastructure.Data;
using getstartedapp.Domain;
using getstartedapp.Services;

namespace getstartedapp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    private readonly IClockService _clock;
    
    private readonly ITodoService _todo;
    
    public ObservableCollection<TodoItem> Items { get; } = new();
    
    public MainWindowViewModel(IClockService clock, ITodoService todo)
    {
        _clock = clock;
        _todo = todo;
    }
    
    // A simple property that uses the injected service
    public string Now => _clock.UtcNow.ToString("u");

    public async Task LoadAsync()
    {
        Items.Clear();
        foreach (var t in await _todo.GetAllAsync())
            Items.Add(t);
    }
    
    public Task AddAsync(string title) => _todo.AddAsync(title);
}

// Derive from your base to keep bindings identical.
public sealed class DesignMainWindowViewModel : MainWindowViewModel
{
    // Provide fakes for design-time only
    private sealed class FakeClock : IClockService
    {
        public DateTime UtcNow => new DateTime(2030, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    }
    
    private sealed class FakeTodo : ITodoService
    {
        public Task<List<TodoItem>> GetAllAsync()
        {
            return Task.FromResult(new List<TodoItem>()
            {
                new TodoItem(1, "One", false),
                new TodoItem(2, "Two", true),
            });
        }

        public Task AddAsync(string title)
        {
            return Task.CompletedTask;
        }
    }
    
    public DesignMainWindowViewModel()
        : base(new FakeClock(), new FakeTodo())
    {
        // Populate more sample data here if needed
    }
}
    
