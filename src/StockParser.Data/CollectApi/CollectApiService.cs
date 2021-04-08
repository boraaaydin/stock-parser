using Newtonsoft.Json;
using StockParser.Data.CollectApi.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public class CollectApiService
    {
        private IHttpClientFactory _clientFactory;

        public CollectApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CollectApiCurrency> GetCurrency()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://api.collectapi.com/economy/currencyToAll?int=1&base=USD");
                request.Headers.Add("Authorization", "apikey 0T0BMnqIoiOz1vzDDnMNEI:559hFvQ8HGp579mT5tPb8y");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString  = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<CollectApiResponse<CollectApiCurrency>>(responseString);
                    if (data.Success)
                    {
                        return data.Result;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
