using MongoDB.Bson;
using MongoDB.Driver;
using StockParser.Domain;
using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.NoSql
{
    public class MongoContextRepository : BaseMongoRepository<StockGroup>
    {
        public MongoContextRepository(IMongoDatabaseSettings settings) : base(settings, "StockContext")
        {
        }

        //public async Task InsertFromContext(List<StockGroup> groups)
        //{
        //    await CreateMany(groups);
        //}

        public async Task<List<StockGroup>> GetStockNameValue()
        {
            return await _entities.Find<StockGroup>(Builders<StockGroup>.Filter.Empty).ToListAsync();

        }

    }
}
