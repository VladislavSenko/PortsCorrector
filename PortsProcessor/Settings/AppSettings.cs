using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PortsProcessor.Settings
{
    public class AppSettings
    {
        public static int ReturnMatches { get; set; }
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static string PortsCollectionName { get; set; }

        public static void LoadSettings(IHostEnvironment hostEnvironment)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json").Build();

            var section = config.GetSection(nameof(AppConfig));
            var appConfig = section.Get<AppConfig>();

            ConnectionString = appConfig.ConnectionString;
            ReturnMatches = appConfig.ReturnMatches;
            DatabaseName = appConfig.DatabaseName;
            PortsCollectionName = appConfig.PortsCollectionName;
        }
    }
}
