using System;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using StockParser.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.ConsoleClient
{
    public class ConsoleClient
    {
        public ConsoleClient(
            ILogger<ConsoleClient> logger, 
            IParser parser,
            BistRepository bistRepo,
            StockRepository stockRepo,
            ParserService parserService)
        {
            _stockRepo = stockRepo;
            _logger = logger;
            _parser = parser;
            _bistRepo = bistRepo;
            _parserService = parserService;
        }
        private List<StockDto> stocks;
        private StockRepository _stockRepo;
        private ILogger<ConsoleClient> _logger;
        private IParser _parser;
        private BistRepository _bistRepo;
        private ParserService _parserService;

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
                                _parserService.CreateStockData();
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
