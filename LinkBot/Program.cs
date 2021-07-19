using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService.Services;

using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using WorkerService.Migrations;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                        // Environment variables need to popular BotConfig__Token
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, serviceCollection) =>
                {
                    var botConfig = hostContext.Configuration.GetSection("BotConfig");
                    // Add functionality to inject IOptions<T>
                    serviceCollection.AddOptions();
                    serviceCollection.Configure<BotConfig>(botConfig);
                    serviceCollection.AddSingleton<LinkMessageHandler>();
                    serviceCollection.AddSingleton<HttpMessageParser>();
                    serviceCollection.AddTransient<LinkAnalyzer>();
                    
                    serviceCollection.AddSingleton<LinkData>();
                    serviceCollection.AddHostedService<Worker>();

                    var connString = botConfig["ConnectionString"];

                    if (connString is null)
                    {
                        throw new Exception("Ensure that connection string is configured in settings.");
                    }
                    
                    serviceCollection.AddFluentMigratorCore()
                        .ConfigureRunner(rb =>
                            rb.AddPostgres()
                                .WithGlobalConnectionString(connString)
                                .ScanIn(typeof(AddLinkTable).Assembly).For
                                .Migrations());
                    
                    
                });
        
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}