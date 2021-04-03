using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using StockParser.NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.NoSql.Services
{
    public class MongoContextService
    {
        private MongoContextRepository _repo;
        public List<StockNameValueDto> StockGroupList { get; set; }

        public MongoContextService(MongoContextRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<StockGroup>> CreateMany(IEnumerable<StockGroup> groups)
        {
            var list = await _repo.CreateMany(groups);
            return list;
        }

        public async Task<List<StockNameValueDto>> GetStockNameValueList(string term)
        {
            if (StockGroupList == null)
            {
                var dbRecords = await _repo.GetStockNameValue();
                dbRecords.ForEach(group => {
                    var list = group?.StockKeyValuelist.Select(f => new StockNameValueDto
                    {
                        GroupName = group.GroupName,
                        StockKey = f.Key,
                        StockName = f.Name
                    });
                    StockGroupList = new List<StockNameValueDto>();
                    StockGroupList.AddRange(list);
                });
            }
            return StockGroupList.Where(x => (
                (x.StockKey !=null && x.StockKey.ToLowerInvariant().Contains(term.ToLowerInvariant())) ||
                ( x.StockName != null && x.StockName.ToLowerInvariant().Contains(term.ToLowerInvariant()))))
                .ToList();
        }
    }
}
