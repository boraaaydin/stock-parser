using StockParser.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Data.WebParser
{
    public interface IWebParser
    {
        Task<HashSet<StockDto>> GetStockData();
    }
}
