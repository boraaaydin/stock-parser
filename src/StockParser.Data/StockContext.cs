using StockParser.Common;
using StockParser.Data.CoinMarketApi;
using StockParser.Data.Services;
using StockParser.Data.WebParser;
using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using StockParser.NoSql.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class StockContext
    {
        public StockContext(IWebParser parser,
            CollectApiService collectApiService,
            CoinMarketService coinMarketService)
        {
            _webParser = parser;
            _collectApiService = collectApiService;
            _coinMarketService = coinMarketService;
        }
        private List<BistStockDto> _Bist;
        private IEnumerable<CurrencyDto> _CurrencyList;
        private CoinMarketModel _Coins;

        private IWebParser _webParser;
        private CollectApiService _collectApiService;
        private CoinMarketService _coinMarketService;

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

        public async Task<IEnumerable<CurrencyDto>> GetDailyCurrencyList()
        {
            if (_CurrencyList == null)
            {
                var currenyList = await _collectApiService.GetCurrency();
                _CurrencyList = currenyList.ConvertToDto();
            }
            return _CurrencyList;
        }

        public async Task<CoinMarketModel> GetCoins()
        {
            if (_Coins == null)
            {
                _Coins = await _coinMarketService.GetMarket();
            }
            return _Coins;
        }
    }
}
