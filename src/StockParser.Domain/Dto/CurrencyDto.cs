using StockParser.Domain.Models;
using System;

namespace StockParser.Domain.Dto
{
    public class CurrencyDto
    {
        public DateTime Date { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string BaseCode { get; set; }
        public decimal Rate { get; set; }

        public StockCodeRate ConvertToDto()
        {
            return new StockCodeRate
            {
                Code = CurrencyCode,
                Rate = Rate
            };
        }
    }
}
