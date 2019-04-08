using Microsoft.Extensions.Logging;
using Stocker.Data.Repository;
using System;

namespace Stocker.Data
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
            Console.WriteLine("Mevcut kolon isimleri çekiliyor...");
            var result = _bistRepo.AddMissingColumns(stocks).Result;
            if (result.Status == ServiceStatus.Ok)
            {
                Console.WriteLine("Eklenen hisseler:" + result.Message);
            }
            else
            {
                Console.WriteLine(result.Message);
            }
            Console.WriteLine("Stok tablosundan son kayıt çekiliyor");
            var lastRecord = _stockRepo.GetLastRecordFromStocks().Result;
            if (lastRecord != null && lastRecord.Date != DateTime.Today ||
                lastRecord == null)
            {
                Console.WriteLine("Stok tablosuna bugün için kayıtlar eklenmemiş, ekleniyor...");
                _stockRepo.AddToStocks(stocks).Wait();
                Console.WriteLine("Stok tablosuna kayıtlar eklendi");
            }
            var lastBistRecord = _bistRepo.GetLastRecordFromStocks().Result;
            if (lastBistRecord != null && lastBistRecord.Date != DateTime.Today ||
                lastBistRecord == null)
            {
                Console.WriteLine("BIST tablosuna yazılıyor");
                _bistRepo.InsertToBIST(stocks).Wait();
                Console.WriteLine("BIST tablosuna kayıtlar eklendi");
            }
        }
    }
}
