using StockParser.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task GetMarket()
        {
            //        var result = await _webService.SendRequest<>>(HttpMethod.Get,
            //"",
            //new Dictionary<string, string>() { { "Authorization", "apikey 0T0BMnqIoiOz1vzDDnMNEI:559hFvQ8HGp579mT5tPb8y" } }
            //);
        }
    }
}
