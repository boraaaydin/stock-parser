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
    public class MongoStockService : BaseMongoRepository<BistStockList>, IStockService
    {
        private IStockRepository _stockRepo;

        public MongoStockService(IStockRepository stockRepo, IMongoDatabaseSettings settings):base(settings)
        {
            _stockRepo = stockRepo;
        }

        public async  Task<ServiceResult<IBistStock>> GetStock(DateTime date, string stockName)
        {
            try
            {
                var stockList = await _stockRepo.GetStockByDate(date);
                return new ServiceResultOk<IBistStock>(stockList?.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult> InsertToStocks(HashSet<IBistStock> list)
        {
            var date = list.FirstOrDefault().Date;
            var entity = new BistStockList
            {
                Date = date,
                BistStocks = list.Select(x => x.ConvertToBistStock())
            };
            var result = await this.Create(entity);
            if (result != null)
            {
                return new ServiceResult(ServiceStatus.Created);
            }
            return new ServiceResult(ServiceStatus.NotCreated);
        }
    }
}
