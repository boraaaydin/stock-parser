using StockParser.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Domain.Dto
{
    public class ProfileDto
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public List<RuleDto> Rules { get; set; }
        public List<OwningDto> Ownings { get; set; }
    }
    public class OwningDto
    {
        public string Name { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal PurchaseCommission { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PurchaseQuantity { get; set; }
        public bool IsSold { get; set; }
        public decimal SellValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public decimal SellCommission { get; set; }
        public DateTime SellDate { get; set; }
        public int SellQuantity { get; set; }
        public decimal? Profit { get; set; }
    }
    public class RuleDto
    {
        public string Name { get; set; }
        public decimal? PurchaseValue { get; set; }
        public decimal? SellValue { get; set; }
        //public RuleMode Mode { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? CurrentValue { get; set; }
    }
}
