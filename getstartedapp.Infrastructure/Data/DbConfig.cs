using System;
using System.IO;

namespace getstartedapp.Infrastructure.Data;

public static class DbConfig
{
    public static string GetConnectionString()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbPath  = Path.Combine(appData, "mega", "getstartedapp", "getstartedapp.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        return $"Data Source={dbPath}"; // Microsoft.Data.Sqlite connection string.
    }
}