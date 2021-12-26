using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PortsProcessor.Data;
using PortsProcessor.Data.Repositories.Implementation;
using PortsProcessor.Data.Repositories.Interfaces;
using PortsProcessor.Providers;
using PortsProcessor.Services;
using PortsProcessor.Settings;
using Serilog;
using Serilog.Events;

namespace PortsProcessor
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static int Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var hostEnvironment = services.GetService<IHostEnvironment>();
                AppSettings.LoadSettings(hostEnvironment);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {   
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddSingleton<IInputPortsRepository, InputPortsRepository>();
                    services.AddSingleton<IPortCodesRepository, PortCodesRepository>();
                    services.AddSingleton<IPortNamesRepository, PortNamesRepository>();
                    services.AddSingleton<IProcessedPortsRepository, ProcessedPortsRepository>();

                    services.AddSingleton<PortsProcessorService>();
                    services.AddSingleton<MatcherProvider>();

                    services.AddScoped<PortService>();

                    services.AddDbContext<PortsDbContext>(
                        option =>
                        {
                            option.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                                .UseSqlServer(AppSettings.ConnectionString);
                        },
                        ServiceLifetime.Singleton);
                });
    }
}
