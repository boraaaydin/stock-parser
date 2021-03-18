using StockParser.Domain;
using StockParser.Domain.Models;
using StockParser.Domain.Services;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.NoSql.Services
{
    public class MongoStockService : IStockService
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

        public async Task<ServiceResult> InsertToStocks(HashSet<BistStockDto> list)
        {
            var date = list.FirstOrDefault().Date;
            var entity = new BistStockList
            {
                Date = date,
                BistStocks = list.Select(x => x.ConvertToBistStock())
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
