using StockParser.Common;
using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Domain.Services
{
    public interface IStockService
    {
        Task<ServiceResult> InsertToStocks(List<StockCodeRate> list);
        Task<ServiceResult<StockCodeRate>> GetStock(DateTime date, String stockName);
    }
}
