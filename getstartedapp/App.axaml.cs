using Autofac;
using Microsoft.EntityFrameworkCore;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System;
using System.IO;
using System.Linq;
using Avalonia.Markup.Xaml;
using getstartedapp.Infrastructure;
using getstartedapp.Infrastructure.Data;
using getstartedapp.Services;
using getstartedapp.ViewModels;
using getstartedapp.Views;


namespace getstartedapp;

public partial class App : Application
{
    private IContainer? _container;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            
            // 1) Build Autofac container
            var builder = new ContainerBuilder();
            
            builder.Register(c => new AppDbContext(DbConfig.GetConnectionString()))
                .AsSelf()
                .InstancePerDependency();
               
            // Register services (singletons by default for app-wide services)
            builder.RegisterType<TodoService>().AsSelf().SingleInstance();
            builder.RegisterType<ClockService>().As<IClockService>().SingleInstance();
            
            // Register view-models (usually transient)
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            // builder.RegisterType<HomeViewModel>().AsSelf();
            // builder.RegisterType<SettingsViewModel>().AsSelf();

            // Register views if you plan to resolve them from the container
            builder.RegisterType<MainWindow>().AsSelf();
            
            _container = builder.Build();
            
            // Apply EF Core migrations at startup (recommended vs EnsureCreated) [3](https://autofac.readthedocs.io/en/latest/examples/index.html)
            using (var scope = _container.BeginLifetimeScope())
            using (var db = scope.Resolve<AppDbContext>())
            {
                db.Database.Migrate();
            }
           
            // 2) Show the main window with a resolved VM
            var vm = _container.Resolve<MainWindowViewModel>();
            desktop.MainWindow = _container.Resolve<MainWindow>();
            desktop.MainWindow.DataContext = vm;

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            // DisableAvaloniaDataAnnotationValidation();
            
            // desktop.MainWindow = new MainWindow
            // {
            //     DataContext = new MainWindowViewModel(),
            // };
        }
        
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}