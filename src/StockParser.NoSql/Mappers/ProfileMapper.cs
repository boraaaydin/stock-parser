using StockParser.Domain.Dto;
using StockParser.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace StockParser.NoSql.Models
{
    public static class ProfileMapper
    {
        public static ProfileDto ConvertToDto(this Profile profile, List<BistStockDto> bist)
        {
            return new ProfileDto
            {
                Id= profile.Id,
                UserId=profile.UserId,
                Rules = profile.Rules.ConvertToDtoList(bist).OrderByDescending(x=>x.Percentage).ToList(),
                Ownings =profile.Ownings.ConvertToDtoList(bist)
            };
        }
    }

}
