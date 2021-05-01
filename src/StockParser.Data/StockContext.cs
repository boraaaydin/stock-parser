using StockParser.Common;
using StockParser.Data.WebParser;
using StockParser.Domain;
using StockParser.Domain.Dto;
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
        public StockContext(IWebParser parser,
            MongoContextService contextService,
            CollectApiService collectApiService)
        {
            _webParser = parser;
            _contextService = contextService;
            _collectApiService = collectApiService;
        }
        private List<BistStockDto> _Bist;
        private IEnumerable<CurrencyDto> _CurrencyList;


        private IWebParser _webParser;
        private MongoContextService _contextService;
        private CollectApiService _collectApiService;

        public async Task<BistStockDto> GetByName(string name)
        {
            return (await GetBist()).FirstOrDefault(x => x.StockName == name);
        }

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

        public async Task<StockGroup> CreateBistKeyGroup()
        {
            var list = (await GetBist()).Select(x => new StockKeyValue
            {
                Key = x.StockName.ToUpper()
            });
            return new StockGroup { GroupName = "Bist", StockKeyValuelist = list.ToList() };
        }

        public async Task<IEnumerable<CurrencyDto>> GetDailyCurrencyList()
        {
            if (_CurrencyList == null)
            {
                var currenyList = await _collectApiService.GetCurrency();
                _CurrencyList = currenyList.ConvertToDto();
            }
            return _CurrencyList;
        }
    }
}
