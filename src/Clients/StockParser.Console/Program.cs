using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Sql;
using StockParser.Sql.Repositories;
using System.Configuration;

namespace StockParser.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var conn = ConfigurationManager.ConnectionStrings["mssql"].ConnectionString;

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(new SqlContext(conn))
                .AddSingleton<IStockRepository, StockRepository>()
                .AddSingleton<IBistRepository, BistRepository>()
                .AddSingleton<ICustomLogger, ConsoleLogger>()
                .AddSingleton<ConsoleClient>()
                .AddScoped<ParserService>()
                .AddSingleton<IWebParser, BigParaParser>()
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
