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
        public ConsoleClient(Repository repo, ILogger<ConsoleClient> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        private List<StockDto> stocks;
        private Repository _repo;
        private ILogger<ConsoleClient> _logger;

        async Task WriteDb()
        {
            Console.WriteLine("Veritabanına yazılıyor...");
            await _repo.WriteAll(stocks);
            Console.WriteLine("Veritabanına yazılma işlemi tamamlandı.");
        }

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
                    Console.WriteLine("1-Data oku");
                    Console.WriteLine("2-STOCK tablosuna kaydet");
                    Console.WriteLine("3-BIST kolonları ekle");
                    Console.WriteLine("4-BIST kayıtları ekle");
                    Console.WriteLine("5-BIST kolonları ekle ve kayıtları ekle");
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
                            Console.WriteLine("Stoklar çekiliyor");
                            IParser parser = new BigParaParser();
                            stocks = await parser.GetData();
                            Console.WriteLine("Stoklar çekildi");
                            break;

                        case "2":
                            Console.Clear();
                            if (stocks == null)
                            {
                                Console.WriteLine("Stock bilgisi bulanamadı");
                                break;
                            }
                            var lastRecord = await _repo.GetLastRecord();
                            if (lastRecord != null)
                            {
                                if (lastRecord.Date != DateTime.Today)
                                {
                                    await WriteDb();
                                }
                                else
                                {
                                    Console.WriteLine("Bugünkü stoklar daha önce çekilmiş");
                                    Console.WriteLine("Yine de çekilsin mi. E/H");
                                    var result = Console.ReadLine();
                                    if (result.ToLower() == "e")
                                    {
                                        await WriteDb();
                                    }
                                }
                            }
                            else
                            {
                                await WriteDb();
                            }
                            break;
                        case "3":
                            {
                                Console.WriteLine("Mevcut kolon isimleri çekiliyor...");
                                var result= await _repo.AddMissingColoumns(stocks);
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
                                Console.WriteLine("Mevcut kolon isimleri çekiliyor...");
                                await _repo.AddMissingColoumns(stocks);
                                Console.WriteLine("BIST tablosuna yazılıyor");
                                await _repo.InsertToBIST(stocks);
                                Console.WriteLine("İşlem tamamlandı");
                                break;
                            }

                    }
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
