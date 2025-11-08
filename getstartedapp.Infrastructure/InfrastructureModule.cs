using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using getstartedapp.Services;
using getstartedapp.Infrastructure.Data;

namespace getstartedapp.Infrastructure;

public class InfrastructureModule : Module
{
    private readonly IConfiguration _configuration;

    public InfrastructureModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TodoService>().As<ITodoService>().SingleInstance();
        builder.RegisterType<ClockService>().As<IClockService>().SingleInstance();
        
        // Decide provider from configuration (e.g., "Sqlite" in appsettings.Development.json, "SqlServer" in prod)
        var provider = _configuration["Provider"] ?? "SqlServer"; // or GetValue<string>("Provider")

        if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
        {
            builder.Register(ctx =>
                {
                    var cs = _configuration.GetConnectionString("Sqlite")
                             ?? "Data Source=./data/dev.db";
                    var options = new DbContextOptionsBuilder<SqliteAppDbContext>()
                        .UseSqlite(cs)
                        .Options;

                    // Construct the provider-specific derived context
                    return new SqliteAppDbContext(options);
                })
                // Expose as base context so the rest of your app can take AppDbContext
                .As<AppDbContext>()
                .As<SqliteAppDbContext>()
                // Scoped per request/unit-of-work
                .InstancePerDependency();
        }
        else
        {
            var test = _configuration["AZURE_SQL_CONNECTION_STRING"];

            
            builder.Register(ctx =>
                {
                  /*  var cs = _configuration.GetConnectionString("AzureSql")
                             ?? throw new InvalidOperationException("Missing ConnectionStrings:AzureSql");
                  */
                  var cs = _configuration["AZURE_SQL_CONNECTION_STRING"];

                    var options = new DbContextOptionsBuilder<SqlServerAppDbContext>()
                        .UseSqlServer(cs)
                        .Options;

                    return new SqlServerAppDbContext(options);
                })
                .As<AppDbContext>()
                .As<SqlServerAppDbContext>()
                .InstancePerDependency();
        }
    }
}