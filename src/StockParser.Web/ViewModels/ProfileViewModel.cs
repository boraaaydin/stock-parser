using StockParser.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Web.ViewModels
{
    public class ProfileViewModel
    {
        public Guid UserId { get; set; }
        public List<RuleViewModel> Rules { get; set; }
        public List<OwningViewModel> Ownings { get; set; }
    }
    public class OwningViewModel
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
    public class RuleViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public RuleMode Mode { get; set; }
    }
}
