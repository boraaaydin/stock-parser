using StockParser.Common;
using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Domain.Services;
using StockParser.NoSql.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class ParserService
    {
        private MongoStockService _mongoStockService;
        private StockContext _stockContext;
        public List<StockDto> stocks;

        //public ICustomLogger _logger { get; }

        public ParserService(
            MongoStockService mongoStockService, 
            StockContext stockContext)
        {
            _mongoStockService = mongoStockService;
            _stockContext = stockContext;
            //_logger = logger;
        }

        public async Task<ServiceResult> InsertStockData()
        {
            var infoMessage = "";
            var errorMessage = "";
            var lastBistRecord = await _mongoStockService.GetStock(DateTime.UtcNow.Date,"ISCTR");
            if (lastBistRecord.Entity == null)
            {
                var currencyDtoList = await _stockContext.GetDailyCurrencyList();
                var bistDtoList = await _stockContext.GetBist();
                var bistResult = await _mongoStockService.InsertToStocks(bistDtoList, currencyDtoList);

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

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new ServiceResult(ServiceStatus.Error, errorMessage);
            }
            return new ServiceResult(ServiceStatus.Ok, infoMessage);

        }
    }
}
