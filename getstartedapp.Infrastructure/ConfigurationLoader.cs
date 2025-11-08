using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace getstartedapp.Infrastructure;

public static class ConfigurationLoader
{
    public static IConfiguration Load()
    {
        //var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "getstarted.Infrastructure");
        var basePath = AppContext.BaseDirectory;
        
        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
         /*   .AddJsonFile("appsettings.Development.json", optional: true)*/
            .AddEnvironmentVariables()
            .Build();
    }
}
