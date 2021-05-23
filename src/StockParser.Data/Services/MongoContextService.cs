using StockParser.Domain.Dto;
using StockParser.NoSql;
using StockParser.NoSql.Models;
using StockParser.NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Data.Services
{
    public class MongoContextService
    {
        private MongoContextRepository _repo;
        private StockContext _stockContext;
        public List<StockNameValueDto> StockGroupList { get; set; }

        public MongoContextService(MongoContextRepository repo, StockContext stockContext)
        {
            _repo = repo;
            _stockContext = stockContext;
        }

        public async Task<IEnumerable<StockGroup>> CreateStockGroups(IEnumerable<StockGroup> groups)
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

        public async Task<StockGroup> CreateBistKeyGroup()
        {
            var list = (await _stockContext.GetBist()).Select(x => new StockKeyValue
            {
                Key = x.StockName.ToUpper()
            });
            return new StockGroup { GroupName = "Bist", StockKeyValuelist = list.ToList() };
        }

    }
}
