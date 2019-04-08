using Microsoft.Extensions.Logging;
using Stocker.Data;
using Stocker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocker
{
    public class ConsoleClient
    {
        public ConsoleClient(
            ILogger<ConsoleClient> logger, 
            IParser parser,
            BistRepository bistRepo,
            StockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _logger = logger;
            _parser = parser;
            _bistRepo = bistRepo;
        }
        private List<StockDto> stocks;
        private StockRepository _stockRepo;
        private ILogger<ConsoleClient> _logger;
        private IParser _parser;
        private BistRepository _bistRepo;

        public async Task Run()
        {
            try
            {
                bool returnBack = true;
                while (returnBack)
                {
                    Console.Clear();
                    Console.WriteLine(stocks == null ? "Stock bilgisi çekilmedi" : "");
                    Console.WriteLine(stocks != null ? "Stocklar mevcut" : "");
                    Console.WriteLine("------------");
                    Console.WriteLine("0-Çıkış");
                    Console.WriteLine("1-Hisseleri Webden oku");
                    Console.WriteLine("2-STOCK tablosuna kaydet");
                    Console.WriteLine("3-BIST eksik kolonları ekle");
                    Console.WriteLine("4-BIST kayıtları kaydet");
                    Console.WriteLine("5-BIST son kayıdı çek");
                    Console.WriteLine("9-TÜMÜNÜ KAYDET");
                    Console.WriteLine("------------");
                    Console.WriteLine("Seçiminizi yapınız...");
                    string secim = Console.ReadLine();

                    switch (secim)
                    {
                        case "0":
                            returnBack = false;
                            break;
                        case "1":
                            Console.Clear();
                            stocks = await _parser.GetData();
                            break;

                        case "2":
                            Console.Clear();
                            if (stocks == null)
                            {
                                Console.WriteLine("Stock bilgisi bulanamadı");
                                break;
                            }
                            var lastRecord = await _stockRepo.GetLastRecordFromStocks();
                            if (lastRecord != null)
                            {
                                if (lastRecord.Date != DateTime.Today)
                                {
                                    await _stockRepo.AddToStocks(stocks);
                                }
                                else
                                {
                                    Console.WriteLine("Bugünkü stoklar daha önce çekilmiş");
                                    Console.WriteLine("Yine de çekilsin mi. E/H");
                                    var result = Console.ReadLine();
                                    if (result.ToLower() == "e")
                                    {
                                        await _stockRepo.AddToStocks(stocks);
                                    }
                                }
                            }
                            else
                            {
                                await _stockRepo.AddToStocks(stocks);
                            }
                            break;
                        case "3":
                            {
                                Console.WriteLine("Mevcut kolon isimleri çekiliyor...");
                                var result = await _bistRepo.AddMissingColumns(stocks);
                                Console.WriteLine("Kolonlar eklenemedi: " + result.Message);
                                break;
                            }
                        case "4":
                            {
                                Console.WriteLine("BIST tablosuna yazılıyor");
                                var result = await _bistRepo.InsertToBIST(stocks);
                                if (result.Status != ServiceStatus.Created)
                                {
                                    Console.WriteLine("hata: " + result.Message);
                                }
                                else
                                {
                                    Console.WriteLine("Başarıyla kaydedildi");
                                }
                                break;
                            }
                        case "9":
                            {
                                stocks = _parser.GetData().Result;
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
                                lastRecord = _stockRepo.GetLastRecordFromStocks().Result;
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
                                break;
                            }

                    }
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
