using StockParser.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Mappers
{
    public class CurrencyMapper
    {
        public static StockDaily ConvertToCurrency(IEnumerable<StockCodeRate> currencyList)
        {
            if (currencyList != null && currencyList.Any())
            {
                return new StockDaily
                {
                    BaseCode = "USD",
                    Data = currencyList.Select(x => new StockCodeRate
                    {
                        Code = x.Code,
                        Rate = x.Rate
                    }).ToList()
                };
            }
            return null;
        }

    }
}

