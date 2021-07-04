using MongoDB.Driver;
using StockParser.NoSql.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.NoSql
{
    public class KeyNameRepository : BaseMongoRepository<KeyNameGroup>
    {
        public KeyNameRepository(IMongoDatabaseSettings settings) : base(settings, "StockContext")
        {
        }

        public async Task<List<KeyNameGroup>> GetKeyNameGroups()
        {
            return await _entities.Find<KeyNameGroup>(Builders<KeyNameGroup>.Filter.Empty).ToListAsync();

        }

    }
}
