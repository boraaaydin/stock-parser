using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.Data.CollectApi.Models
{
    public class CollectApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("result")]
        public T Result { get; set; }
    }
    public class CollectApiCurrency
    {
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("lastUpdate")]
        public string LastUpdate { get; set; }
        [JsonProperty("data")]
        public List<CurrencyResponse> CurrencyList { get; set; }
    }

    public class CurrencyResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("rate")]
        public double Rate { get; set; }
        [JsonProperty("calculatedstr")]
        public double Calculatedstr { get; set; }
        [JsonProperty("calculated")]
        public double Calculated { get; set; }
    }
}
