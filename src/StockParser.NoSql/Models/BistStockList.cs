using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public class BistStockList:BaseMongoModel
    {
        [BsonElement("Date")]
        public DateTime Date { get; set; }
        public IEnumerable<BistStock> BistStocks { get; set; }
        public CurrencyDaily Currency { get; internal set; }
    }
}
