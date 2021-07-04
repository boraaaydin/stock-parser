using StockParser.Data.CoinMarketApi;
using StockParser.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Mappers
{
    public class CoinMapper
    {
        public static StockDaily ConvertToDailyCoin(IEnumerable<StockCodeRate> coinDtoList)
        {
            if (coinDtoList != null && coinDtoList.Any())
            {
                return new StockDaily
                {
                    BaseCode = "USD",
                    Data = coinDtoList.Select(x => new StockCodeRate
                    {
                        Code = x.Code,
                        Rate = x.Rate,
                    }).ToList()
                };
            }
            return null;
        }

        public static IEnumerable<StockCodeRate> ConvertToDto(CoinMarketModel data)
        {
            return data.data.Select(x => new StockCodeRate
            {
                Code = x.symbol,
                Rate = x.quote.USD.price
            });
        }
    }
}
