using StockParser.Domain;
using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Domain
{
    public interface IStockRepository
    {
        //Task<ServiceResult> InsertToStocks(HashSet<StockDto> list);
        //Task<StockDto> GetTodaysRecordFromStocks(string stockName);
        //Task<IEnumerable<StockDto>> GetTodaysRecordsFromStocks();
        Task<List<BistStockDto>> GetStockByDate(DateTime date);
    }
}
