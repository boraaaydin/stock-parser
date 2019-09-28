using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Moq;
using StockParser.Data;
using StockParser.Data.WebParser;
using StockParser.Function.Infrastructure;
using StockParser.Sql;
using StockParser.Sql.Repositories;

namespace StockParser.Function
{
    public static class ParserFunction
    {
        [FunctionName("Parsetimer")]
        public static void Run(
            [TimerTrigger("0 00 21 * * *")]TimerInfo myTimer,
            //[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ILogger log)
        {
            StockParserService.Execute(log);
            //log.LogInformation($"Parser executed at: {DateTime.Now}");
            //var webParser = new BigParaParser();
            //var connString = "Server=tcp:boraydin.database.windows.net,1433;Initial Catalog=serverless_db;Persist Security Info=False;User ID=superadmin;Password=epcRdAQ5Bpq3PPnX;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //var sqlContext = new SqlContext(connString);
            //var bistRepo = new BistRepository( sqlContext);
            //var stockRepo = new StockRepository( sqlContext);
            //var parserService = new ParserService(webParser, bistRepo, stockRepo);
            
            //var result=parserService.InsertStockData().Result;
            //if (result.Status == Common.ServiceStatus.Ok)
            //{
            //    log.LogInformation($"Success: {DateTime.Now}");
            //}
            //else
            //{
            //    log.LogInformation($"Fail: {result.Message} Time:{DateTime.Now}");
            //}
        }
    }
}
