using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public class Rule
    {
        public string Name { get; set; }
        public decimal? PurchaseValue { get; set; }
        public decimal? SellValue { get; set; }
    }
}
