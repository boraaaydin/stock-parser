using StockParser.Domain;
using System;

namespace StockParser.NoSql.Models
{
    public class BistStock //: BaseMongoModel
    {
        public string StockName { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal YesterdayPrice { get; set; }
        public decimal DailyChange { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public long VolumeLot { get; set; }
        public long VolumeTL { get; set; }
        //public DateTime Date { get; set; }
        //public DateTime CreatedAt { get; set; }
    }
}
