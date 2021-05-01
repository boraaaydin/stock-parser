using StockParser.Common;
using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using StockParser.NoSql.Mappers;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.NoSql.Services
{
    public class MongoStockService 
    {
        private MongoStockRepository _stockRepo;

        public MongoStockService(MongoStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
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

        public async Task<ServiceResult> InsertToStocks(List<BistStockDto> list, IEnumerable<CurrencyDto> currency)
        {
            var date = list.FirstOrDefault().Date;
            var entity = new BistStockList
            {
                Date = date,
                BistStocks = list.Select(x => x.ConvertToBistStock()),
                Currency = CurrencyMapper.ConvertToCurrency(currency)
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
