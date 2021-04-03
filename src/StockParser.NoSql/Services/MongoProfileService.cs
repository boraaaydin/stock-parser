using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using StockParser.NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.NoSql.Services
{
    public class MongoProfileService
    {
        private MongoProfileRepository _repo;

        public MongoProfileService(MongoProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<Profile> CreateProfile(Guid userId)
        {
            var profile = new Profile
            {
                UserId = userId,
                Ownings = new List<Owning>(),
                Rules = new List<Rule>()
            };
            var list = await _repo.Create(profile);
            return list;
        }


        public async Task<Profile> GetProfile(Guid userId)
        {
            var profile = await _repo.GetByUserId(userId);
            if (profile == null)
            {
                profile = await CreateProfile(userId);
            }
            return profile;
        }

        public async Task<Profile> InsertOwning(Guid userId, OwningDto owning)
        {
            var profile = await _repo.GetByUserId(userId);
            profile.Ownings.Add(owning.Convert());
            await _repo.Update(profile.Id, profile);
            return profile;
        }

        public async Task<Profile> InsertRule(Guid userId, RuleDto rule)
        {
            var profile = await _repo.GetByUserId(userId);
            profile.Rules.Add(rule.Convert());
            await _repo.Update(profile.Id, profile);
            return profile;
        }
    }
}
