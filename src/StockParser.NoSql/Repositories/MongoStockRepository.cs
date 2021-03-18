using MongoDB.Bson;
using MongoDB.Driver;
using StockParser.Domain;
using StockParser.Domain.Models;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.NoSql
{
    public class MongoStockRepository : BaseMongoRepository<BistStockList>, IStockRepository
    {
        public MongoStockRepository(IMongoDatabaseSettings settings) : base(settings, "Stocks")
        {
        }

        public async Task<List<BistStockDto>> GetStockByDate(DateTime date)
        {
            //return  await _entities.FindAsync<BistStockList>(entity => entity.Date == date);
            //var filter= Builders.Filter<BistStockList>.Eq(x=>x.);
            var builder = Builders<BistStockList>.Filter;
            var filter = builder.Eq("Date", date);
            var list = await _entities.Find(filter)?.FirstOrDefaultAsync();
            return list.ConvertToList();
        }

        //public Task<StockDto> GetTodaysRecordFromStocks(string stockName)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<StockDto>> GetTodaysRecordsFromStocks()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<ServiceResult> InsertToStocks(HashSet<StockDto> list)
        //{
        //    var date = DateTime.Now.Date;
        //    var entity = new BistStockList
        //    {
        //        Date = date,
        //        BistStocks = list.Select(x => new BistStock { AveragePrice = x.AveragePrice })
        //    };
        //    var result = await this.Create(entity);
        //    if (result != null)
        //    {
        //        return new ServiceResult(ServiceStatus.Created);
        //    }
        //    return new ServiceResult(ServiceStatus.NotCreated);
        //}
    }
}
