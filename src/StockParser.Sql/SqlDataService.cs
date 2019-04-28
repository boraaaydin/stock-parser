//using Microsoft.Extensions.Logging;
//using StockParser.Common;
//using StockParser.Sql.Repositories;
//using System;

//namespace StockParser.Sql
//{
//    public class SqlDataService
//    {
//        private IParser _parser;
//        private BistRepository _bistRepo;
//        private StockRepository _stockRepo;

//        public SqlDataService(ILogger<SqlDataService> logger,
//            IParser parser,
//            BistRepository bistRepo,
//            StockRepository stockRepo)
//        {
//            _parser = parser;
//            _bistRepo = bistRepo;
//            _stockRepo=stockRepo;
//        }
//        public void CreateStockData()
//        {
//            var stocks = _parser.GetData().Result;
           
//            Console.WriteLine("Checking if todays stocks have already been inserted");
//            var lastRecord = _stockRepo.GetLastRecordFromStocks().Result;
//            if (lastRecord != null && lastRecord.Date != DateTime.Today ||
//                lastRecord == null)
//            {
//                _stockRepo.AddToStocks(stocks).Wait();
//                Console.WriteLine("Adding stocks to STOCKS table have been completed");
//            }
//            var lastBistRecord = _bistRepo.GetLastRecordFromStocks().Result;
//            if (lastBistRecord != null && lastBistRecord.Date != DateTime.Today ||
//                lastBistRecord == null)
//            {
//                _bistRepo.InsertToBIST(stocks).Wait();
//                Console.WriteLine("Adding stocks to BIST table have been completed");
//            }
//        }
//    }
//}
