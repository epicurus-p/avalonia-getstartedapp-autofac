using Avalonia.Controls;
using getstartedapp.Domain;
using getstartedapp.ViewModels;

namespace getstartedapp.Views;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnLoadClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
            await vm.LoadAsync();
    }

    private async void OnAddClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && this.FindControl<TextBox>("Input") is { } tb)
        {
            if (!string.IsNullOrWhiteSpace(tb.Text))
                await vm.AddAsync(tb.Text.Trim());
            tb.Text = "";
            await vm.LoadAsync();
        }
    }
}
