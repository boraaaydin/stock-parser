using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.NoSql.Models
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public double Rate { get; set; }
    }

    public class CurrencyDaily
    {
        public string BaseCode { get; set; }
        public List<Currency> CurrencyList { get; set; }
    }
}
