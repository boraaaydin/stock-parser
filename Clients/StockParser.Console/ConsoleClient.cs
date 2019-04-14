using System;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using StockParser.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockParser.Data.WebParser;

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
                    Console.WriteLine(stocks == null ? "Could not parse stocks" : "");
                    Console.WriteLine(stocks != null ? "Stocks are present" : "");
                    Console.WriteLine("------------");
                    Console.WriteLine("0-Exit");
                    Console.WriteLine("1-Parse Stocks");
                    Console.WriteLine("2-Insert stocks to STOCK table");
                    Console.WriteLine("3-Add missing colums to BIST table");
                    Console.WriteLine("4-Insert stocks to BIST table");
                    Console.WriteLine("5-Get last row from BIST table");
                    Console.WriteLine("9-Insert all stocks to BIST and STOCK tables");
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
                            stocks = await _parser.GetData();
                            break;

                        case "2":
                            Console.Clear();
                            if (stocks == null)
                            {
                                Console.WriteLine("Could not find stock");
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
                                    Console.WriteLine("Todays stocks have already been parsed ");
                                    Console.WriteLine("Parse anyway. y/n");
                                    var result = Console.ReadLine();
                                    if (result.ToLower() == "y")
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
                                Console.WriteLine("Getting column names from BIST table...");
                                var result = await _bistRepo.AddMissingColumns(stocks);
                                break;
                            }
                        case "4":
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
                        case "9":
                            {
                                _parserService.CreateStockData();
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
            }
        }
    }
}
