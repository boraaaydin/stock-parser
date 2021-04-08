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

}
