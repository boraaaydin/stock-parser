using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Mappers
{
    public class CurrencyMapper
    {
        public static CurrencyDaily ConvertToCurrency(IEnumerable<CurrencyDto> currencyList)
        {
            if (currencyList != null && currencyList.Any())
            {
                return new CurrencyDaily
                {
                    BaseCode = currencyList.FirstOrDefault().BaseCode,
                    CurrencyList = currencyList.Select(x => new Currency
                    {
                        CurrencyCode = x.CurrencyCode,
                        Rate = x.Rate
                    }).ToList()
                };
            }
            return null;
        }

    }
}

