using System;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Common;

namespace StockParser.ConsoleClient
{
    public class ConsoleClient
    {
        public ConsoleClient(
            ILogger<ConsoleClient> logger, 
            IWebParser parser,
            IBistRepository bistRepo,
            IStockRepository stockRepo,
            ParserService parserService)
        {
            _stockRepo = stockRepo;
            _logger = logger;
            _parser = parser;
            _bistRepo = bistRepo;
            _parserService = parserService;
        }
        private HashSet<StockDto> stocks;
        private IStockRepository _stockRepo;
        private ILogger<ConsoleClient> _logger;
        private IWebParser _parser;
        private IBistRepository _bistRepo;
        private ParserService _parserService;

        public async Task Run()
        {
            
            try
            {
                bool returnBack = true;
                while (returnBack)
                {
                    Console.Clear();
                    Console.WriteLine(stocks == null ? "Could not parse stocks" : "");
                    Console.WriteLine(stocks != null ? "Stocks are present" : "");
                    Console.WriteLine("------------");
                    Console.WriteLine("0-Exit");
                    Console.WriteLine("1-Parse Stocks from Web");
                    Console.WriteLine("2-Insert stocks to STOCKS");
                    Console.WriteLine("3-Insert stocks to BIST");
                    Console.WriteLine("4-Get last row from STOCKS");
                    Console.WriteLine("9-Insert all stocks to BIST and STOCKS");
                    Console.WriteLine("------------");
                    Console.WriteLine("Make your choise...");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "0":
                            returnBack = false;
                            break;
                        case "1":
                            Console.Clear();
                            stocks = await _parser.GetStockData();
                            break;

                        case "2":
                            Console.Clear();
                            if (stocks == null)
                            {
                                Console.WriteLine("Could not find stock");
                                break;
                            }
                            var lastRecord = await _stockRepo.GetTodaysRecordFromStocks();
                            if (lastRecord != null)
                            {
                                if (lastRecord.Date != DateTime.Today)
                                {
                                    await _stockRepo.InsertToStocks(stocks);
                                }
                                else
                                {
                                    Console.WriteLine("Todays stocks have already been parsed ");
                                    Console.WriteLine("Parse anyway. y/n");
                                    var result = Console.ReadLine();
                                    if (result.ToLower() == "y")
                                    {
                                        await _stockRepo.InsertToStocks(stocks);
                                    }
                                }
                            }
                            else
                            {
                                await _stockRepo.InsertToStocks(stocks);
                            }
                            break;
                        case "3":
                            {
                                Console.WriteLine("Inserting to BIST table...");
                                var result = await _bistRepo.InsertToBIST(stocks);
                                if (result.Status != ServiceStatus.Created)
                                {
                                    Console.WriteLine("error: " + result.Message);
                                }
                                else
                                {
                                    Console.WriteLine("Inserted with Success");
                                }
                                break;
                            }
                        case "4":
                            {
                                Console.WriteLine("Getting last row from STOCKS table");
                                lastRecord = await _stockRepo.GetTodaysRecordFromStocks();
                                if (lastRecord == null)
                                {
                                    Console.WriteLine("Last record received null");
                                }
                                else
                                {
                                    if (lastRecord.Date.Equals(DateTime.Today))
                                    {
                                        Console.WriteLine("Find record for today");
                                    }
                                    else
                                    {
                                        Console.WriteLine("There is not any record for today");
                                    }
                                }
                                break;
                            }
                        case "9":
                            {
                                _parserService.InsertStockData().Wait();
                                break;
                            }

                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
