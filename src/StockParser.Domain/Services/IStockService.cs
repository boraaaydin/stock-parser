using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Domain.Services
{
    public interface IStockService
    {
        Task<ServiceResult> InsertToStocks(HashSet<IBistStock> list);
        Task<ServiceResult<IBistStock>> GetStock(DateTime date, String stockName);
    }
}
