using System.Collections.Generic;

namespace StockParser.Domain.Models
{
    public class StockDaily
    {
        public string BaseCode { get; set; }
        public List<StockCodeRate> Data { get; set; }
    }

    public class StockCodeRate
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
