using StockParser.Common;
using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using StockParser.NoSql;
using StockParser.NoSql.Mappers;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Data.Services
{
    public class StockService 
    {
        private MongoStockRepository _stockRepo;
        private ContextService _contextService;

        public StockService(MongoStockRepository stockRepo, ContextService stockContext)
        {
            _stockRepo = stockRepo;
            _contextService = stockContext;
        }

        public async Task<ServiceResult> InsertAllData()
        {
            var infoMessage = "";
            var errorMessage = "";
            var lastBistRecord = await GetStock(DateTime.UtcNow.Date, "ISCTR");
            if (lastBistRecord.Entity == null)
            {
                var allResult = await InsertAllStocks();

                if (allResult.Status == ServiceStatus.Error)
                {
                    errorMessage += allResult.Message;
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

        public async  Task<ServiceResult<StockCodeRate>> GetStock(DateTime date, string stockName)
        {
            try
            {
                var stockData = await _stockRepo.GetStockByDate(date);
                return new ServiceResultOk<StockCodeRate>(stockData?.BistStocks?.Data.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult> InsertAllStocks()
        {
            var currencyList = await GetLatestCurrency();
            var bistList = await GetLatestBist();
            var coinDtoList = await GetLatestCoinData();

            var date = DateTime.UtcNow.Date; // bistList.FirstOrDefault().Date;
            var entity = new StockData
            {
                Date = date,
                BistStocks = BistStockMapper.ConvertToBistStock(bistList),
                Currency = CurrencyMapper.ConvertToCurrency(currencyList),
                Coins = CoinMapper.ConvertToDailyCoin(coinDtoList)
            };
            var result = await _stockRepo.Create(entity);
            if (result != null)
            {
                return new ServiceResult(ServiceStatus.Created);
            }
            return new ServiceResult(ServiceStatus.NotCreated);
        }

        public async Task<IEnumerable<StockCodeRate>> GetLatestCurrency()
        {
            var data = await _contextService.GetDailyCurrencyList();
            var dto = data.Select(x => x.ConvertToDto());
            return dto;
        }

        public async Task<IEnumerable<StockCodeRate>> GetLatestCoinData()
        {
            var coinMarketData = await _contextService.GetCoins();
            var coinDto = CoinMapper.ConvertToDto(coinMarketData);
            return coinDto;
        }

        public async Task<IEnumerable<StockCodeRate>> GetLatestBist()
        {
            var bistData = await _contextService.GetBist();
            return bistData.Select(x => x.ConvertToDto());
        }
    }
}
