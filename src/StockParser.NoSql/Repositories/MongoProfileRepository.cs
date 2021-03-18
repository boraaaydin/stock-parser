using MongoDB.Driver;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.NoSql.Repositories
{
    public class MongoProfileRepository : BaseMongoRepository<Profile>
    {
        public MongoProfileRepository(IMongoDatabaseSettings settings) : base(settings, "Profiles")
        {
        }

        public async Task<Profile> GetByUserId(Guid id)
        {
            var builder = Builders<Profile>.Filter;
            var filter = builder.Eq("UserId", id);
            var entity = await _entities.Find(filter)?.FirstOrDefaultAsync();
            return entity;
        }

        public async Task<Profile> AddOrUpdateRules(Guid userId, List<Rule> newRules)
        {
            var userRules = await GetByUserId(userId);
            if (userRules == null)
            {
                var entity = new Profile
                {
                    UserId = userId,
                    Rules = newRules
                };
                await _entities.InsertOneAsync(entity);
                return entity;
            }
            else
            {
                userRules.Rules = newRules;
                return userRules;
            }
        }

              
    }
}
