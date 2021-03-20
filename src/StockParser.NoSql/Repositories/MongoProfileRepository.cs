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
            var profile = await GetByUserId(userId);
            if (profile == null)
            {
                var entity = new Profile
                {
                    UserId = userId,
                    Rules = newRules,
                    Ownings = new List<Owning>()
                };
                await _entities.InsertOneAsync(entity);
                return entity;
            }
            else
            {
                profile.Rules = newRules;
                await Update(profile.Id, profile);
                return profile;
            }
        }

              
    }
}
