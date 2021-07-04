using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockParser.NoSql.Models
{
    public static class OwningMapper
    {
        public static List<OwningDto> ConvertToDtoList(this List<Owning> ownings, List<StockCodeRate> bist)
        {
            var owningDtos = new List<OwningDto>();
            ownings.ForEach(x => owningDtos.Add(x.ConvertToDto(bist)));
            return owningDtos;
        }

        public static OwningDto ConvertToDto(this Owning owning, List<StockCodeRate> bist)
        {
            return new OwningDto
            {
                Name = owning.Name,
                SellCommission = owning.SellCommission,
                SellDate = owning.SellDate,
                PurchaseDate = owning.PurchaseDate,
                SellValue = owning.SellValue,
                PurchaseCommission = owning.PurchaseCommission,
                PurchaseQuantity = owning.PurchaseQuantity,
                PurchaseValue = owning.PurchaseValue,
                CurrentValue = bist.FirstOrDefault(x => x.Code == owning.Name)?.Rate,
                Profit = CalculateProfit(owning,bist)
            };
        }

        private static decimal? CalculateProfit(Owning owning, List<StockCodeRate> bist)
        {
            var stock = bist.FirstOrDefault(x => x.Code == owning.Name);
            if (stock != null)
            {
                var currentValue = stock.Rate;
                var purchaseValue = owning.PurchaseValue;
                return owning.PurchaseQuantity * (currentValue - purchaseValue);
            }
            return null;
        }
    }


    public static class OwningDtoMapper
    {
        public static Owning Convert(this OwningDto owning)
        {
            return new Owning
            {
                Name = owning.Name,
                SellCommission = owning.SellCommission,
                SellDate = owning.SellDate,
                PurchaseDate = owning.PurchaseDate,
                //SellQuantity = owning.SellQuantity,
                SellValue = owning.SellValue,
                //sIsSold = owning.IsSold,
                PurchaseCommission = owning.PurchaseCommission,
                PurchaseQuantity = owning.PurchaseQuantity,
                PurchaseValue = owning.PurchaseValue
            };
        }
    }
}
