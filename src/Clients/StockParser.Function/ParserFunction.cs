using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using StockParser.Function.Infrastructure;

namespace StockParser.Function
{
    public static class ParserFunction
    {
        [FunctionName("ParserService")]
        public static void Run([TimerTrigger("0 30 22 1/1 * ? *")]TimerInfo myTimer, 
            ILogger log,
            [Inject] ParserService parserService)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            parserService.InsertStockData().Wait();
        }
    }
}
