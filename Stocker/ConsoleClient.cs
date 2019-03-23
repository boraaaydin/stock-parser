using Stocker.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocker
{
    public class ConsoleClient
    {
        public ConsoleClient()
        {
            repo = new Repository();
        }
        private static List<StockDto> stocks;
        private static Repository repo;

        static async Task WriteDb()
        {
            Console.WriteLine("Veritabanına yazılıyor...");
            await repo.WriteAll(stocks);
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
                    Console.WriteLine("1-data oku");
                    Console.WriteLine("2-veritabanına yaz");
                    Console.WriteLine("3-çıkış");
                    Console.WriteLine("------------");
                    Console.WriteLine("Seçiminizi yapınız...");
                    string secim = Console.ReadLine();

                    switch (secim)
                    {
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
                            var lastRecord = await repo.GetLastRecord();
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
                            returnBack = false;
                            break;
                    }
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
