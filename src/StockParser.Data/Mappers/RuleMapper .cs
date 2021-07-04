using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Models
{
    public static class RuleMapper
    {
        public static List<RuleDto> ConvertToDtoList(this List<Rule> rules, List<StockCodeRate> bist)
        {
            var rulesDto = new List<RuleDto>();
            rules.ForEach(x => rulesDto.Add(x.ConvertToDto(bist.FirstOrDefault(y => y.Code == x.Name))));
            return rulesDto;
        }
        public static RuleDto ConvertToDto(this Rule rule, StockCodeRate bist)
        {
            var ruleDto =  new RuleDto
            {
                Name = rule.Name,
                PurchaseValue = rule.PurchaseValue,
                SellValue = rule.SellValue,
                CurrentValue = bist?.Rate
            };
            if (bist?.Rate != null)
            {
                if (rule.PurchaseValue != null)
                {
                    ruleDto.Percentage = (rule.PurchaseValue - bist?.Rate) / bist.Rate;
                }
            }
            return ruleDto;
        }
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
