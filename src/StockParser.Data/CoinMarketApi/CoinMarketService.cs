using StockParser.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Data.CoinMarketApi
{
    public class CoinMarketService
    {
        private WebService _webService;

        public CoinMarketService(WebService webService)
        {
            _webService = webService;
        }

        public async Task<CoinMarketModel> GetMarket()
        {
            var url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";
            var headers = new Dictionary<string, string>()
            {
                { "X-CMC_PRO_API_KEY", "6a7e8f1e-df94-4219-a3c1-bbaa5fc45427" },
                { "Accepts", "application/json"}
            };
            var result = await _webService.SendRequest<CoinMarketModel>(HttpMethod.Get, url, headers);
            if (result.Status == ServiceStatus.Ok)
            {
                return result.Entity;
            }
            return null;
        }
    }
}
