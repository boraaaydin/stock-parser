using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockParser.NoSql.Models
{
    public static class BistStockMapper
    {
        public static  BistStock ConvertToBistStock(this IBistStock stock)
        {
            return new BistStock
            {
                StockName = stock.StockName,
                DailyChange = stock.DailyChange,
                AveragePrice=stock.AveragePrice,
                FinalPrice=stock.FinalPrice,
                HighestPrice=stock.HighestPrice,
                LowestPrice=stock.LowestPrice,
                VolumeLot=stock.VolumeLot,
                VolumeTL=stock.VolumeTL,
                YesterdayPrice=stock.YesterdayPrice
            };
        }

        public static List<IBistStock> ConvertToList(this BistStockList stockList)
        {
            return stockList?.BistStocks.Select(stock => new IBistStock {
                StockName = stock.StockName,
                DailyChange = stock.DailyChange,
                AveragePrice = stock.AveragePrice,
                FinalPrice = stock.FinalPrice,
                HighestPrice = stock.HighestPrice,
                LowestPrice = stock.LowestPrice,
                VolumeLot = stock.VolumeLot,
                VolumeTL = stock.VolumeTL,
                YesterdayPrice = stock.YesterdayPrice,
                Date=stockList.Date
            }).ToList();
        }
    }
}
