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
    
    /*
    public static string GetConnectionString()
    {
        return $"@Server=tcp:sqlfree-server7b709ef4.database.windows.net,1433;Initial Catalog=sqlfree-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";"; // AzureSQL Connection String
    }
    */
}