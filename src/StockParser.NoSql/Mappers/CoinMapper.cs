using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Mappers
{
    public class CoinMapper
    {
        public static StockDaily ConvertToDailyCoin(IEnumerable<CoinDto> coinDtoList)
        {
            if (coinDtoList != null && coinDtoList.Any())
            {
                return new StockDaily
                {
                    BaseCode = "USD",
                    StockList = coinDtoList.Select(x => new StockCodeRate
                    {
                        Code = x.Symbol,
                        Rate = x.Price,
                    }).ToList()
                };
            }
            return null;
        }
    }
}
