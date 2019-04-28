using StockParser.Common;
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

        public async Task<ServiceResult> InsertStockData()
        {
            var infoMessage = "";
            var errorMessage = "";
            var lastBistRecord = await _bistRepo.GetTodaysRecordFromStocks();
            if (lastBistRecord == null)
            {
                var stocks = await _webParser.GetStockData();       
                var bistResult= await _bistRepo.InsertToBIST(stocks);
                if (bistResult.Status == ServiceStatus.Error)
                {
                    errorMessage += bistResult.Message;
                }
                else
                {
                    infoMessage += "Bist data inserted";
                }
            }
            else
            {
                infoMessage += "Bist data has already been inserted. ";
            }
            var lastStockRecord = await _stockRepo.GetTodaysRecordFromStocks();
            if (lastStockRecord == null)
            {
                var stocks = await _webParser.GetStockData();
                var stockResult = await _stockRepo.InsertToStocks(stocks);
                if (stockResult.Status == ServiceStatus.Error)
                {
                    errorMessage += stockResult.Message;
                }
                else
                {
                    infoMessage += "Stock data inserted";
                }
            }
            else
            {
                infoMessage += "Stock data has already been inserted";
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new ServiceResult(ServiceStatus.Error, errorMessage);
            }
            return new ServiceResult(ServiceStatus.Ok, infoMessage);

        }
    }
}
