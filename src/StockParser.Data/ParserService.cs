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
        private IStockService _stockService;
        private StockContext _stockContext;
        public List<StockDto> stocks;

        //public ICustomLogger _logger { get; }

        public ParserService(IStockService stockService, StockContext stockContext)
        {
            _stockService = stockService;
            _stockContext = stockContext;
            //_logger = logger;
        }

        public async Task<ServiceResult> InsertStockData()
        {
            var infoMessage = "";
            var errorMessage = "";
            var lastBistRecord = await _stockService.GetStock(DateTime.UtcNow.Date,"ISCTR");
            if (lastBistRecord.Entity == null)
            {
                    var bistResult = await _stockService.InsertToStocks(await _stockContext.GetBist());
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
