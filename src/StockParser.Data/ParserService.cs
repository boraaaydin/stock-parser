using Microsoft.Extensions.Logging;
using StockParser.Data.Repository;
using StockParser.Data.WebParser;
using System;

namespace StockParser.Data
{
    public class ParserService
    {
        private IParser _parser;
        private BistRepository _bistRepo;
        private StockRepository _stockRepo;

        public ParserService(ILogger<ParserService> logger,
            IParser parser,
            BistRepository bistRepo,
            StockRepository stockRepo)
        {
            _parser = parser;
            _bistRepo = bistRepo;
            _stockRepo=stockRepo;
        }
        public void CreateStockData()
        {
            var stocks = _parser.GetData().Result;
            Console.WriteLine("Checking if new columns to be added");
            var result = _bistRepo.AddMissingColumns(stocks).Result;
            if (result.Status == ServiceStatus.Ok)
            {
                Console.WriteLine("New stock names:" + result.Message);
            }
            else
            {
                Console.WriteLine(result.Message);
            }
            Console.WriteLine("Checking if todays stocks have already been inserted");
            var lastRecord = _stockRepo.GetLastRecordFromStocks().Result;
            if (lastRecord != null && lastRecord.Date != DateTime.Today ||
                lastRecord == null)
            {
                _stockRepo.AddToStocks(stocks).Wait();
                Console.WriteLine("Adding stocks to STOCKS table have been completed");
            }
            var lastBistRecord = _bistRepo.GetLastRecordFromStocks().Result;
            if (lastBistRecord != null && lastBistRecord.Date != DateTime.Today ||
                lastBistRecord == null)
            {
                _bistRepo.InsertToBIST(stocks).Wait();
                Console.WriteLine("Adding stocks to BIST table have been completed");
            }
        }
    }
}
