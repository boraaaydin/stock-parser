using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocker.Data;
using System;
using System.Collections.Generic;

namespace Stocker
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<Repository>()
                .AddSingleton<ConsoleClient>()
                //.AddSingleton<IBarService, BarService>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var consoleClient = serviceProvider.GetService<ConsoleClient>();
            consoleClient.Run().Wait();

            //logger.LogDebug("All done!");
            //var client = new ConsoleClient();
            //client.Run().Wait();
        }       
    }
}
