using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using StockParser.NoSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockParser.NoSql.Models
{
    public static class RuleMapper
    {
        public static List<RuleDto> ConvertToDtoList(this List<Rule> rules, List<BistStockDto> bist)
        {
            var rulesDto = new List<RuleDto>();
            rules.ForEach(x => rulesDto.Add(x.ConvertToDto(bist.FirstOrDefault(y => y.StockName == x.Name))));
            return rulesDto;
        }
        public static RuleDto ConvertToDto(this Rule rule, BistStockDto bist)
        {
            var ruleDto =  new RuleDto
            {
                //Mode = rule.Mode,
                Name = rule.Name,
                PurchaseValue = rule.PurchaseValue,
                CurrentValue = bist?.FinalPrice
            };
            if (bist?.FinalPrice != null)
            {
                if (rule.PurchaseValue != null)
                {
                    ruleDto.Percentage = (rule.PurchaseValue - bist?.FinalPrice) / bist.FinalPrice;
                }
                //TODO
                //if(rule.SellValue != null)
                //{

                //}
            }
            return ruleDto;
        }

        //private static decimal CalculatePercentage (Rule rule, BistStockDto bist)
        //{
        //    if(Decimal.TryParse(rule.Value, out decimal ruleValue))
        //    {

        //    }
        //}
    }


    public static class RuleDtoMapper
    {
        public static Rule Convert(this RuleDto rule)
        {
            return new Rule
            {
                Name = rule.Name,
                PurchaseValue=rule.PurchaseValue,
                SellValue=rule.SellValue
            };
        }
    }
}
