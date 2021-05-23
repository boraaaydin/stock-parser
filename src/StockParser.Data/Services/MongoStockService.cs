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
    public class MongoStockService 
    {
        private MongoStockRepository _stockRepo;
        private StockContext _stockContext;

        public MongoStockService(MongoStockRepository stockRepo, StockContext stockContext)
        {
            _stockRepo = stockRepo;
            _stockContext = stockContext;
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

        public async  Task<ServiceResult<BistStockDto>> GetStock(DateTime date, string stockName)
        {
            try
            {
                var stockList = await _stockRepo.GetStockByDate(date);
                return new ServiceResultOk<BistStockDto>(stockList?.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult> InsertAllStocks()
        {
            var currencyList = new List<CurrencyDto>();// await _stockContext.GetDailyCurrencyList();
            var bistList = new List<BistStockDto>();// await _stockContext.GetBist();
            var coinDtoList = await GetLatestCoinData();

            var date = DateTime.Now.Date; // bistList.FirstOrDefault().Date;
            var entity = new BistStockList
            {
                Date = date,
                BistStocks = bistList.Select(x => x.ConvertToBistStock()),
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

        public async Task<IEnumerable<CoinDto>> GetLatestCoinData()
        {
            var coinMarketData = await _stockContext.GetCoins();
            var coinDto = coinMarketData.ConvertToDto();
            return coinDto;
        }
    }
}
