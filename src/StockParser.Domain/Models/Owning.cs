using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public class Owning : ICloneable
    {
        public string name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("İsim boş olamaz");
                }
                return name;
            }
            set
            {
                name = value;
            }
        }
        public decimal PurchaseValue { get; set; }
        public decimal PurchaseCommission { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int PurchaseQuantity { get; set; }
        public decimal SellValue { get; set; }
        public decimal SellCommission { get; set; }
        public DateTime SellDate { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone(); ;
        }
    }


}
