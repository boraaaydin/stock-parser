using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.NoSql.Models
{
    public static class OwningMapper
    {
        public static List<OwningDto> ConvertToDtoList(this List<Owning> ownings)
        {
            var owningDtos = new List<OwningDto>();
            ownings.ForEach(x => owningDtos.Add(x.ConvertToDto()));
            return owningDtos;
        }
        public static OwningDto ConvertToDto(this Owning owning)
        {
            return new OwningDto
            {
                Name = owning.Name,
                SellCommission = owning.SellCommission,
                SellDate = owning.SellDate,
                PurchaseDate = owning.PurchaseDate,
                SellQuantity = owning.SellQuantity,
                SellValue = owning.SellValue,
                IsSold = owning.IsSold,
                PurchaseCommission = owning.PurchaseCommission,
                PurchaseQuantity = owning.PurchaseQuantity,
                PurchaseValue = owning.PurchaseValue
            };
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
                SellQuantity = owning.SellQuantity,
                SellValue = owning.SellValue,
                IsSold = owning.IsSold,
                PurchaseCommission = owning.PurchaseCommission,
                PurchaseQuantity = owning.PurchaseQuantity,
                PurchaseValue = owning.PurchaseValue
            };
        }
    }
}
