using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Domain.Models;
using StockParser.NoSql.Models;
using StockParser.NoSql.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class StockContext
    {
        public StockContext(IWebParser parser, MongoContextService contextService)
        {
            _webParser = parser;
            _contextService = contextService;
        }
        private List<BistStockDto> _Bist;
        private IWebParser _webParser;
        private MongoContextService _contextService;

        public async Task<List<BistStockDto>> GetBist()
        {
            if (_Bist == null)
            {
                var stocksResult = await _webParser.GetStockData();
                if (stocksResult.Status == ServiceStatus.Ok)
                {
                    _Bist = stocksResult.Entity;
                }
            }
            return _Bist;
        }

        public async Task InsertContextKeyNameValues()
        {
            var contextGroup = new List<StockGroup>
            {
                await CreateBistKeyGroup()
            };
            await _contextService.CreateMany(contextGroup);
        }

        private async Task<StockGroup> CreateBistKeyGroup()
        {
            var list = (await GetBist()).Select(x => new StockKeyValue
            {
                Key = x.StockName.ToUpper()
            });
            return new StockGroup { GroupName = "Bist", StockKeyValuelist = list.ToList() };
        }
    }
}
