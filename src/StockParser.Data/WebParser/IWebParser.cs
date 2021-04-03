using StockParser.Common;
using StockParser.Domain;
using StockParser.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Data.WebParser
{
    public interface IWebParser
    {
        Task<ServiceResult<List<BistStockDto>>> GetStockData();
    }
}
