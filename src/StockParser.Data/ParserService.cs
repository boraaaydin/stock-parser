using StockParser.Common;
using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class ParserService
    {
        private IWebParser _webParser;
        private IStockService _stockService;
        public List<StockDto> stocks;

        //public ICustomLogger _logger { get; }

        public ParserService(IWebParser webParser, IStockService stockService)
        {
            _webParser = webParser;
            _stockService = stockService;
            //_logger = logger;
        }

        public async Task<ServiceResult> InsertStockData()
        {
            var infoMessage = "";
            var errorMessage = "";
            var lastBistRecord = await _stockService.GetStock(DateTime.UtcNow.Date,"ISCTR");
            if (lastBistRecord.Entity == null)
            {
                var stocksResult = await _webParser.GetStockData();
                if (stocksResult.Status != ServiceStatus.Ok)
                {
                    errorMessage += stocksResult.Message;
                }
                else
                {
                    var bistResult = await _stockService.InsertToStocks(stocksResult.Entity);
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

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new ServiceResult(ServiceStatus.Error, errorMessage);
            }
            return new ServiceResult(ServiceStatus.Ok, infoMessage);

        }
    }
}
