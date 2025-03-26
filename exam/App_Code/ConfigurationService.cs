using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Summary description for ConfigurationService
/// </summary>
public class ConfigurationService
{
    private static readonly IConfiguration _configuration;

    static ConfigurationService()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(basePath, "appsettings.json"), optional: false, reloadOnChange: true)
            .Build();
    }

    public static string dbLogin
    {
        get { return _configuration["Database:Login"]; }
    }

    public static string dbPassword
    {
        get { return _configuration["Database:Password"]; }
    }
}
