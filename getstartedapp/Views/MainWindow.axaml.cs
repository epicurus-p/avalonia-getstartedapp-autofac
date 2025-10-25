using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using getstartedapp.ViewModels;

namespace getstartedapp.Views;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Hook window lifecycle
        this.Opened += OnOpened;

        // Optional: centralized hotkeys (we also have KeyBindings in XAML)
        this.KeyDown += OnWindowKeyDown;

        // Hook DataGrid editing events
        if (this.FindControl<DataGrid>("TodoGrid") is { } grid)
        {
            grid.RowEditEnded += OnGridRowEditEnded;
            grid.CellEditEnded += OnGridCellEditEnded;
        }
    }

    // Load initial data when the window is shown
    private async void OnOpened(object? sender, EventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            // Prefer executing the generated RelayCommand so CanExecute is honored
            await vm.LoadCommand.ExecuteAsync(null);

            // Optional: put keyboard focus in the quick-add box
            this.FindControl<TextBox>("QuickAdd")?.Focus();
        }
    }

    // Global shortcuts: Ctrl+F focuses search; Delete deletes selected
    private async void OnWindowKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;

        if (e.Key == Key.F && e.KeyModifiers == KeyModifiers.Control)
        {
            this.FindControl<TextBox>("SearchBox")?.Focus();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Delete)
        {
            if (vm.DeleteSelectedCommand.CanExecute(null))
            {
                await vm.DeleteSelectedCommand.ExecuteAsync(null);
                e.Handled = true;
            }
        }
    }

    // When a row edit finishes (e.g., Title / Priority / Due / Notes changed), save it
    private async void OnGridRowEditEnded(object? sender, DataGridRowEditEndedEventArgs e)
    {
        var rowVm = e.Row?.DataContext as TodoItemViewModel;
        if (rowVm is null) return;

        if (DataContext is MainWindowViewModel vm)
        {
            // Validate and persist the row via the ViewModel command
            await vm.SaveRowCommand.ExecuteAsync(rowVm);
        }
    }

    // When a single cell edit ends, handle special cases (checkbox toggle -> ToggleDoneAsync)
    private async void OnGridCellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        var rowVm = e.Row?.DataContext as TodoItemViewModel;
        if (rowVm is null) return;

        if (DataContext is MainWindowViewModel vm)
        {
            // If the edited column is the Done checkbox, call ToggleDone
            // We can detect via column type or header; type is more robust:
            if (e.Column is DataGridCheckBoxColumn)
            {
                await vm.ToggleDoneCommand.ExecuteAsync(rowVm);
            }
            else
            {
                // For text/date/priority edits you can save on cell end as well
                // (or rely on RowEditEnded only for fewer saves).
                // await vm.SaveRowAsyncCommand.ExecuteAsync(rowVm);
            }
        }
    }

    // Optional: handlers for the "Add" and "Load" buttons if you prefer code-behind events
    private async void OnAddClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
            await vm.AddCommand.ExecuteAsync(null);
    }

    private async void OnLoadClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
            await vm.LoadCommand.ExecuteAsync(null);
    }
}
