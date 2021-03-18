using StockParser.Domain.Dto;
using StockParser.NoSql.Models;
using System.Collections.Generic;

namespace StockParser.NoSql.Models
{
    public static class ProfileMapper
    {
        public static ProfileDto ConvertToDto(this Profile profile)
        {
            return new ProfileDto
            {
                Id= profile.Id,
                UserId=profile.UserId,
                Rules = profile.Rules.ConvertToDtoList(),
                Ownings =profile.Ownings.ConvertToDtoList()
            };
        }
    }

    public static class RuleMapper
    {
        public static List<RuleDto> ConvertToDtoList(this List<Rule> rules)
        {
            var rulesDto = new List<RuleDto>();
            rules.ForEach(x => rulesDto.Add(x.ConvertToDto()));
            return rulesDto;
        }
        public static RuleDto ConvertToDto(this Rule rule)
        {
            return new RuleDto
            {
                Mode=rule.Mode,
                Name=rule.Name,
                Value=rule.Value
            };
        }
    }

   
}
