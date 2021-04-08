using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Domain.Dto
{
    public class CurrencyDto
    {
        public DateTime Date { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string BaseCode { get; set; }
        public double Rate { get; set; }
    }
}
