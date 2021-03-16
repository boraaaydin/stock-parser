using MongoDB.Driver;
using StockParser.NoSql.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.NoSql
{
    public class BaseMongoRepository<T> where T:BaseMongoModel
    {
        public readonly IMongoCollection<T> _entities;

        public BaseMongoRepository(IMongoDatabaseSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _entities = database.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> Get() =>
            (await _entities.FindAsync(entity => true)).ToList();

        public async Task<T> Get(string id) =>
           (await _entities.FindAsync<T>(entity => entity.Id == id)).FirstOrDefault();

        public async Task<T> Create(T entity)
        {
            await _entities.InsertOneAsync(entity);
            return entity;
        }

        public async Task Update(string id, T entityIn) =>
            await _entities.ReplaceOneAsync(entity => entity.Id == id, entityIn);

        public async Task Remove(T entityIn) =>
            await _entities.DeleteOneAsync(entity => entity.Id == entityIn.Id);

        public async Task Remove(string id) =>
            await _entities.DeleteOneAsync(entity => entity.Id == id);
    }
}
