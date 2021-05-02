using Newtonsoft.Json;
using StockParser.Common;
using StockParser.Data.CollectApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class CollectApiService
    {
        private WebService _webService;

        public CollectApiService(WebService webService)
        {
            _webService = webService;
        }

        public async Task<CollectApiCurrency> GetCurrency()
        {
            var url = "https://api.collectapi.com/economy/currencyToAll?int=1&base=USD";
            var headers = new Dictionary<string, string>() { { "Authorization", "apikey 0T0BMnqIoiOz1vzDDnMNEI:559hFvQ8HGp579mT5tPb8y" } };
            var result = await _webService.SendRequest<CollectApiResponse<CollectApiCurrency>>(HttpMethod.Get, url, headers);

            if (result.Status == ServiceStatus.Ok)
            {
                if (result.Entity.Success)
                {
                    return result.Entity.Result;
                }
            }
            return null;
        }
    }
}
