using Microsoft.Extensions.Logging;
using Stocker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocker
{
    public class ConsoleClient
    {
        public ConsoleClient(Repository repo, ILogger<ConsoleClient> logger, IParser parser)
        {
            _repo = repo;
            _logger = logger;
            _parser = parser;
        }
        private List<StockDto> stocks;
        private Repository _repo;
        private ILogger<ConsoleClient> _logger;
        private IParser _parser;

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
                    Console.WriteLine("5-Eksik kolonları ekle ve kayıtları kaydet");
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
                            var lastRecord = await _repo.GetLastRecordFromStocks();
                            if (lastRecord != null)
                            {
                                if (lastRecord.Date != DateTime.Today)
                                {
                                    await _repo.AddToStocks(stocks);
                                }
                                else
                                {
                                    Console.WriteLine("Bugünkü stoklar daha önce çekilmiş");
                                    Console.WriteLine("Yine de çekilsin mi. E/H");
                                    var result = Console.ReadLine();
                                    if (result.ToLower() == "e")
                                    {
                                        await _repo.AddToStocks(stocks);
                                    }
                                }
                            }
                            else
                            {
                                await _repo.AddToStocks(stocks);
                            }
                            break;
                        case "3":
                            {
                                Console.WriteLine("Mevcut kolon isimleri çekiliyor...");
                                var result = await _repo.AddMissingColoumns(stocks);
                                Console.WriteLine("Kolonlar eklenemedi: " + result.Message);
                                break;
                            }
                        case "4":
                            {
                                Console.WriteLine("BIST tablosuna yazılıyor");
                                var result = await _repo.InsertToBIST(stocks);
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
                        case "5":
                            {
                                stocks = _parser.GetData().Result;
                                Console.WriteLine("Mevcut kolon isimleri çekiliyor...");
                                var result = _repo.AddMissingColoumns(stocks).Result;
                                if (result.Status == ServiceStatus.Ok)
                                {
                                    Console.WriteLine("Eklenen hisseler:" + result.Message);
                                }
                                else
                                {
                                    Console.WriteLine(result.Message);
                                }
                                Console.WriteLine("Stok tablosundan son kayıt çekiliyor");
                                lastRecord = _repo.GetLastRecordFromStocks().Result;
                                if (lastRecord != null && lastRecord.Date != DateTime.Today ||
                                    lastRecord == null)
                                {
                                    Console.WriteLine("Bugün için kayıtlar eklenmemiş, ekleniyor...");
                                    _repo.AddToStocks(stocks).Wait();
                                    Console.WriteLine("Stok tablosuna kayıtlar eklendi");

                                    Console.WriteLine("BIST tablosuna yazılıyor");
                                    _repo.InsertToBIST(stocks).Wait();
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
