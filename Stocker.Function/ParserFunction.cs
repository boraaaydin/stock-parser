using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Stocker.Data;
using Stocker.Function.Infrastructure;

namespace Stocker.Function
{
    public static class ParserFunction
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 35 21 1/1 * ? *")]TimerInfo myTimer, ILogger log,
            [Inject] ParserService parserService)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            parserService.CreateStockData();
        }
    }
}
