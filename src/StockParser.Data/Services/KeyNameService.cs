using StockParser.Domain.Dto;
using StockParser.NoSql;
using StockParser.NoSql.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Data.Services
{
    public class KeyNameService
    {
        private KeyNameRepository _repo;
        private ContextService _stockContext;
        public List<StockNameValueDto> StockGroupList { get; set; }

        public KeyNameService(KeyNameRepository repo, ContextService stockContext)
        {
            _repo = repo;
            _stockContext = stockContext;
        }

        public async Task<IEnumerable<KeyNameGroup>> CreateStockGroups(IEnumerable<KeyNameGroup> groups)
        {
            var list = await _repo.CreateMany(groups);
            return list;
        }

        public async Task<List<StockNameValueDto>> GetStockNameValueList(string term)
        {
            if (StockGroupList == null)
            {
                var keyNameGroups = await _repo.GetKeyNameGroups();
                keyNameGroups.ForEach(group => {
                    var list = group?.KeyNameList.Select(f => new StockNameValueDto
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

        public async Task<KeyNameGroup> CreateBistKeyGroup()
        {
            var list = (await _stockContext.GetBist()).Select(x => new KeyNameItem
            {
                Key = x.StockName.ToUpper()
            });
            return new KeyNameGroup { GroupName = "Bist", KeyNameList = list.ToList() };
        }

    }
}
