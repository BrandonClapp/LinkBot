using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService.Services;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                        // Environment variables need to popular BotConfig__Token
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Add functionality to inject IOptions<T>
                    services.AddOptions();
                    services.Configure<BotConfig>(hostContext.Configuration.GetSection("BotConfig"));
                    
                    services.AddSingleton<LinkMessageHandler>();
                    services.AddSingleton<HttpMessageParser>();
                    
                    services.AddTransient<LinkAnalyzer>();
                    
                    services.AddHostedService<Worker>();
                });
    }
}