using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockParser.NoSql.Models
{
    public static class BistStockMapper
    {
        //public static BistStock ConvertToBistStock(this BistStockDto stock)
        //{
        //    return new BistStock
        //    {
        //        StockName = stock.StockName,
        //        DailyChange = stock.DailyChange,
        //        AveragePrice = stock.AveragePrice,
        //        FinalPrice = stock.FinalPrice,
        //        HighestPrice = stock.HighestPrice,
        //        LowestPrice = stock.LowestPrice,
        //        VolumeLot = stock.VolumeLot,
        //        VolumeTL = stock.VolumeTL,
        //        YesterdayPrice = stock.YesterdayPrice
        //    };
        //}

        //public static List<BistStockDto> ConvertToList(this StockData stockList)
        //{
        //    return stockList?.BistStocks.Select(stock => new BistStockDto
        //    {
        //        StockName = stock.StockName,
        //        DailyChange = stock.DailyChange,
        //        AveragePrice = stock.AveragePrice,
        //        FinalPrice = stock.FinalPrice,
        //        HighestPrice = stock.HighestPrice,
        //        LowestPrice = stock.LowestPrice,
        //        VolumeLot = stock.VolumeLot,
        //        VolumeTL = stock.VolumeTL,
        //        YesterdayPrice = stock.YesterdayPrice,
        //        Date = stockList.Date
        //    }).ToList();
        //}
        public static StockDaily ConvertToBistStock(IEnumerable<StockCodeRate> stocks)
        {
            return new StockDaily
            {
                BaseCode = "TRY",
                Data = stocks.ToList()
            };
        }

    }

}
