using StockParser.Common;
using StockParser.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Data.WebParser
{
    public interface IWebParser
    {
        Task<ServiceResult<HashSet<StockDto>>> GetStockData();
    }
}
