using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocker.Data;
using Stocker.Data.Repository;
using System;
using System.Collections.Generic;

namespace Stocker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<StockRepository>()
                .AddSingleton<BistRepository>()
                .AddSingleton<ConsoleClient>()
                .AddSingleton<IParser, BigParaParser>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Trace);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var consoleClient = serviceProvider.GetService<ConsoleClient>();
            consoleClient.Run().Wait();
        }

        //private static void ConfigureServices(IServiceCollection services)
        //{
        //    services
        //      .AddLogging(opt =>
        //      {
        //          opt.AddConsole();
        //          opt.SetMinimumLevel(LogLevel.Trace);
        //      });
        //}
    }
}
