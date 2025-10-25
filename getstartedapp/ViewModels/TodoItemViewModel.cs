using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using getstartedapp.Domain;

namespace getstartedapp.ViewModels;

// Wraps the entity for validation and edit-friendly binding
public partial class TodoItemViewModel : ObservableValidator
{
    public int Id { get; private set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required, MinLength(2), MaxLength(200)]
    private string _title = "";

    [ObservableProperty]
    private bool _isDone;

    [ObservableProperty]
    private TodoPriority _priority = TodoPriority.Medium;

    [ObservableProperty]
    [CustomValidation(typeof(TodoItemViewModel), nameof(ValidateDueDate))]
    private DateTimeOffset? _dueDate;

    [ObservableProperty] private string? _notes;
    
    public IReadOnlyList<TodoPriority> TodoPriorities { get; } = Enum.GetValues<TodoPriority>();

    public static ValidationResult? ValidateDueDate(DateTimeOffset? value, ValidationContext _)
    {
        if (value is null) return ValidationResult.Success;
        var today = DateTime.Today;
        return value.Value.Date < today ? new ValidationResult("Due date cannot be in the past.") : ValidationResult.Success;
    }

    public static TodoItemViewModel FromEntity(TodoItem e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        IsDone = e.IsDone,
        Priority = e.Priority,
        DueDate = e.DueDate?.ToLocalTime().Date,
        Notes = e.Notes
    };

    public TodoItem ToEntity() => new()
    {
        Id = Id, Title = Title, IsDone = IsDone, Priority = Priority, DueDate = DueDate?.UtcDateTime, Notes = Notes
    };

    public void ValidateAll() => ValidateAllProperties();
}