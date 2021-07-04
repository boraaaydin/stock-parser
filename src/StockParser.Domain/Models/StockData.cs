using MongoDB.Bson.Serialization.Attributes;
using StockParser.Domain.Models;
using System;
using System.Collections.Generic;

namespace StockParser.NoSql.Models
{
    public class StockData : BaseMongoModel
    {
        [BsonElement("Date")]
        public DateTime Date { get; set; }
        public StockDaily BistStocks { get; set; }
        public StockDaily Currency { get; set; }
        public StockDaily Coins { get; set; }
    }
}
