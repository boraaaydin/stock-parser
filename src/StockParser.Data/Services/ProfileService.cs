using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using StockParser.NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Data.Services
{
    public class ProfileService
    {
        private MongoProfileRepository _repo;

        public ProfileService(MongoProfileRepository repo)
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

        public async Task<Profile> AddOrUpdateRule(Guid userId, RuleDto ruleDto)
        {
            var profile = await _repo.GetByUserId(userId);
            var existingRule = profile.Rules.FirstOrDefault(x => x.Name == ruleDto.Name);
            if (existingRule != null)
            {
                existingRule.PurchaseValue = ruleDto.PurchaseValue;
                existingRule.SellValue = ruleDto.SellValue;
            }
            else
            {
                profile.Rules.Add(ruleDto.Convert());
            }
            await _repo.Update(profile.Id, profile);
            return profile;
        }


        public async Task<Rule> GetRule(Guid userId, string stockName)
        {
            var profile = await _repo.GetByUserId(userId);
            var rule = profile.Rules.Where(x => x.Name == stockName).FirstOrDefault();
            return rule;
        }

        public async Task SellOwning(Guid userId, OwningDto owning)
        {
            var profile = await _repo.GetByUserId(userId);
            SellFromOwning(profile, owning);
            await _repo.Update(profile.Id, profile);
        }

        public void SellFromOwning(Profile profile, OwningDto soldOwning)
        {
            bool isFirst = true;
            int totalOwningCount = 0;
            var ownings = profile.Ownings.Where(x => x.Name == soldOwning.Name).OrderBy(x => x.PurchaseDate).ToList();
            ownings.ForEach(x => totalOwningCount += x.PurchaseQuantity);
            if (totalOwningCount < soldOwning.PurchaseQuantity)
            {
                throw new Exception("Sahip olunandan daha fazla satış yapılamaz");
            }
            var soldOwnings = new List<Owning>();
            Owning remainingOwning = null;
            var remainingOwningList = new List<Owning>();
            int remainingQuantity= soldOwning.PurchaseQuantity;
            foreach(var owning in ownings)
            {
                if (owning.PurchaseQuantity <= remainingQuantity)
                {
                    owning.SellDate = soldOwning.SellDate;
                    owning.SellValue = soldOwning.SellValue;
                    remainingQuantity -= owning.PurchaseQuantity;
                    soldOwnings.Add(owning);
                }
                else
                {
                    if (isFirst)
                    {
                        remainingOwning = (Owning)owning.Clone();
                        remainingOwning.PurchaseQuantity -= remainingQuantity;
                        remainingOwningList.Add(remainingOwning);
                        isFirst = false;
                    }
                    else
                    {
                        remainingOwningList.Add(owning);
                    }
                }
            }

            profile.Ownings.RemoveAll(x => x.Name == soldOwning.Name);
            if (remainingOwningList.Count>0)
            {
                profile.Ownings.AddRange(remainingOwningList);
            }
            profile.Solds.AddRange(soldOwnings);
        }

    }
}
