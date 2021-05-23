using System.Collections.Generic;

namespace StockParser.NoSql.Models
{
    public class StockDaily
    {
        public string BaseCode { get; set; }
        public List<StockCodeRate> StockList { get; set; }
    }

    public class StockCodeRate
    {
        public string Code { get; set; }
        public double Rate { get; set; }
    }
}
