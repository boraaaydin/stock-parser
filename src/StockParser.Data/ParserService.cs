using StockParser.Data.WebParser;
using StockParser.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class ParserService
    {
        private IBistRepository _bistRepo;
        private IWebParser _webParser;
        private IStockRepository _stockRepo;
        public List<StockDto> stocks;

        public ParserService(IWebParser webParser, IBistRepository bistRepo, IStockRepository stockRepo)
        {
            _bistRepo = bistRepo;
            _webParser = webParser;
            _stockRepo = stockRepo;
        }

        public async Task InsertStockData()
        {
            var lastBistRecord = await _bistRepo.GetTodaysRecordFromStocks();
            if (lastBistRecord == null)
            {
                var stocks = await _webParser.GetStockData();       
                await _bistRepo.InsertToBIST(stocks);
            }
            var lastStockRecord = await _stockRepo.GetTodaysRecordFromStocks();
            if (lastStockRecord == null)
            {
                var stocks = await _webParser.GetStockData();
                await _stockRepo.InsertToStocks(stocks);
            }

        }
    }
}
