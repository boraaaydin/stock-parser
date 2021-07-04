using Newtonsoft.Json;
using StockParser.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.Data.CollectApi.Models
{
    public class CollectApiCurrency
    {
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("lastUpdate")]
        public string LastUpdate { get; set; }
        [JsonProperty("data")]
        public List<CurrencyResponse> CurrencyList { get; set; }

        public IEnumerable<CurrencyDto> ConvertToDto()
        {
            return this.CurrencyList.Select(x => new CurrencyDto
            {
                BaseCode = this.Base,
                Date = DateTime.UtcNow, // Convert.ToDateTime(this.LastUpdate),
                CurrencyCode = x.Code,
                CurrencyName=x.Name,
                Rate=x.Rate
            });
        }
    }

    public class CurrencyResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("rate")]
        public decimal Rate { get; set; }
        [JsonProperty("calculatedstr")]
        public double Calculatedstr { get; set; }
        [JsonProperty("calculated")]
        public double Calculated { get; set; }
    }
}
