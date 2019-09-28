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

        public ICustomLogger _logger { get; }

        public ParserService(IWebParser webParser, IBistRepository bistRepo, IStockRepository stockRepo, ICustomLogger logger)
        {
            _bistRepo = bistRepo;
            _webParser = webParser;
            _stockRepo = stockRepo;
            _logger = logger;
        }

        public async Task<ServiceResult> InsertStockData()
        {

            var infoMessage = "";
            var errorMessage = "";
            var lastBistRecord = await _bistRepo.GetTodaysRecordFromStocks();
            if (lastBistRecord == null)
            {
                var stocksResult = await _webParser.GetStockData();
                if (stocksResult.Status != ServiceStatus.Ok)
                {
                    errorMessage += stocksResult.Message;
                }
                else
                {
                    var bistResult = await _bistRepo.InsertToBIST(stocksResult.Entity);
                    if (bistResult.Status == ServiceStatus.Error)
                    {
                        errorMessage += bistResult.Message;
                    }
                    else
                    {
                        infoMessage += "Bist data inserted";
                    }
                }
            }
            else
            {
                infoMessage += "Bist data has already been inserted. ";
            }
            var lastStockRecord = await _stockRepo.GetTodaysRecordFromStocks();
            if (lastStockRecord == null)
            {
                var stocksResult = await _webParser.GetStockData();
                if (stocksResult.Status != ServiceStatus.Ok)
                {
                    errorMessage += stocksResult.Message;
                }
                else
                {
                    var stockResult = await _stockRepo.InsertToStocks(stocksResult.Entity);
                    if (stockResult.Status == ServiceStatus.Error)
                    {
                        errorMessage += stockResult.Message;
                    }
                    else
                    {
                        infoMessage += "Stock data inserted";
                    }
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
