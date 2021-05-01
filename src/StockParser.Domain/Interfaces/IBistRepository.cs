using StockParser.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockParser.Domain
{
    public interface IBistRepository
    {
        Task<StockDto> GetTodaysRecordFromStocks();
        Task<ServiceResult> InsertToBIST(HashSet<StockDto> list);
    }
}
