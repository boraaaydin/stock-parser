using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StockParser.Common
{
    public class WebService
    {
        private IHttpClientFactory _clientFactory;

        public WebService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ServiceResult<T>> SendRequest<T>(HttpMethod method, string url, Dictionary<string,string> headers)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                if (headers != null)
                {
                    foreach (KeyValuePair<string,string> header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(responseString);
                    return new ServiceResult<T>(ServiceStatus.Ok, data);
                }
                return new ServiceResult<T>(ServiceStatus.NOk, response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                return new ServiceResult<T>(ServiceStatus.NOk, ex.Message);
            }
        }
    }
}
