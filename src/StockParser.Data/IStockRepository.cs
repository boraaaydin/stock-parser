using StockParser.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public interface IStockRepository
    {
        Task InsertToStocks(HashSet<StockDto> list);
        Task<StockDto> GetTodaysRecordFromStocks();
    }
}
