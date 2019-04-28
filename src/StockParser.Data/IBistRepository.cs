using StockParser.Common;
using StockParser.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public interface IBistRepository
    {
        Task<StockDto> GetTodaysRecordFromStocks();
        Task<ServiceResult> InsertToBIST(HashSet<StockDto> list);
    }
}
