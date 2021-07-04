using StockParser.Domain.Dto;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Models
{
    public static class ProfileMapper
    {
        public static ProfileDto ConvertToDto(this Profile profile, List<WebBistDto> bist)
        {
            return new ProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Rules = profile.Rules.ConvertToDtoList(bist.Select(x=>x.ConvertToDto()).ToList()).OrderByDescending(x => x.Percentage).ToList(),
                Ownings = profile.Ownings.ConvertToDtoList(bist.Select(x=>x.ConvertToDto()).ToList())
            };
        }
    }

}
