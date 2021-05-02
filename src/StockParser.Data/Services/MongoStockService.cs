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

        public async Task<ServiceResult> InsertToStocks()
        {
            var currencyList = await _stockContext.GetDailyCurrencyList();
            var bistList = await _stockContext.GetBist();

            var date = bistList.FirstOrDefault().Date;
            var entity = new BistStockList
            {
                Date = date,
                BistStocks = bistList.Select(x => x.ConvertToBistStock()),
                Currency = CurrencyMapper.ConvertToCurrency(currencyList)
            };
            var result = await _stockRepo.Create(entity);
            if (result != null)
            {
                return new ServiceResult(ServiceStatus.Created);
            }
            return new ServiceResult(ServiceStatus.NotCreated);
        }
    }
}
