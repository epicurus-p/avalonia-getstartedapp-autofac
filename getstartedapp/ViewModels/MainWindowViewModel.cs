using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    
    public IReadOnlyList<TodoFilter> Filters { get; } = Enum.GetValues<TodoFilter>();
    
    public IReadOnlyList<TodoPriority> TodoPriorities { get; } = Enum.GetValues<TodoPriority>();
    
    public ObservableCollection<TodoItemViewModel> Items { get; } = new();
    [ObservableProperty] private TodoItemViewModel? _selected;
    [ObservableProperty] private TodoFilter _filter = TodoFilter.All;
    [ObservableProperty] private string? _search;

    // Quick-add fields    
    [ObservableProperty] private string _newTitle = "";
    [ObservableProperty] private TodoPriority _newPriority = TodoPriority.Medium;
    [ObservableProperty] private DateTimeOffset? _newDue;
    
    public int CompletedCount => Items.Count(i => i.IsDone);
    public int TotalCount => Items.Count;
    
    public MainWindowViewModel(IClockService clock, ITodoService todo)
    {
        _clock = clock;
        _todo = todo;
    }
    
    // A simple property that uses the injected service
    public string Now => _clock.UtcNow.ToString("u");

    [RelayCommand]
    public async Task LoadAsync()
    {
        var list = await _todo.QueryAsync(Filter, Search);
        Items.Clear();
        foreach (var e in list) Items.Add(TodoItemViewModel.FromEntity(e));
        OnPropertyChanged(nameof(CompletedCount));
        OnPropertyChanged(nameof(TotalCount));
    }

    [RelayCommand]
    public async Task AddAsync()
    {
        var vm = new TodoItemViewModel { Title = NewTitle, Priority = NewPriority, DueDate = NewDue?.DateTime };
        vm.ValidateAll();
        if (vm.HasErrors) return;
        var result = await _todo.AddAsync(vm.Title, vm.Priority, vm.DueDate?.DateTime, vm.Notes);
        Items.Insert(0, TodoItemViewModel.FromEntity(result));
        NewTitle = "";
        NewPriority = TodoPriority.Medium;
        NewDue = null;
        OnPropertyChanged(nameof(TotalCount));
    }

    [RelayCommand]
    public async Task ToggleDoneAsync(TodoItemViewModel vm)
    {
        await _todo.ToggleDoneAsync(vm.Id, vm.IsDone);
        OnPropertyChanged(nameof(CompletedCount));
    }
    
    [RelayCommand]
    public async Task SaveRowAsync(TodoItemViewModel vm)
    {
        vm.ValidateAll();
        if (vm.HasErrors) return;
        await _todo.UpdateAsync(vm.ToEntity());
        await LoadAsync(); // simple refresh; or update in-place for better perf
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    public async Task DeleteSelectedAsync()
    {
        if (Selected is null) return;
        await _todo.DeleteAsync(Selected.Id);
        Items.Remove(Selected);
        OnPropertyChanged(nameof(CompletedCount));
        OnPropertyChanged(nameof(TotalCount));
    }
    public bool CanDelete => Selected is not null;

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
        private static List<TodoItem> Items => new()
        {
            new TodoItem(1, "One", false),
            new TodoItem(2, "Two", true),
        };
        public Task<List<TodoItem>> GetAllAsync()
        {
            return Task.FromResult(Items);
        }

        public Task<IReadOnlyList<TodoItem>> QueryAsync(
            TodoFilter filter,
            string? search,
            CancellationToken ct = default)
        {
            return Task.FromResult(Items as IReadOnlyList<TodoItem>);
        }

        public Task<TodoItem> AddAsync(string title, TodoPriority priority, DateTime? dueDate, string? notes,
            CancellationToken ct = default)
        {
            return Task.FromResult(new TodoItem(1, "One", false));
        }
        public Task ToggleDoneAsync(int id, bool isDone, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
        public Task UpdateAsync(TodoItem updated, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
        public Task DeleteAsync(int id, CancellationToken ct = default)
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

    
