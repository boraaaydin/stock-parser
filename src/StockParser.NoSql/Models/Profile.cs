using StockParser.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public class Profile : BaseMongoModel
    {
        public Guid UserId { get; set; }
        public List<Rule> Rules { get; set; }
        public List<Owning> Ownings { get; set; }
    }

    public class Owning
    {
        public string Name { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal PurchaseCommission { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PurchaseQuantity { get; set; }
        public bool IsSold { get; set; }
        public decimal SellValue { get; set; }
        public decimal SellCommission { get; set; }
        public DateTime SellDate { get; set; }
        public int SellQuantity { get; set; }
    }

    public class Rule
    {
        public string Name { get; set; }
        public decimal? PurchaseValue { get; set; }
        public decimal? SellValue { get; set; }
    }

}
