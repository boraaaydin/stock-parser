using Dapper;
using Microsoft.Extensions.Logging;
using StockParser.Common;
using StockParser.Data;
using StockParser.Domain;
using StockParser.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace StockParser.Sql.Repositories
{
    public class StockRepository : BaseRepository, IStockRepository
    {
        private readonly ICustomLogger _logger;

        public StockRepository(SqlContext context, ICustomLogger logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<StockDto> GetTodaysRecordFromStocks(string stockName)
        {
            _logger.LogTrace("Getting last record from Stock Table");
            using (SqlConnection conn = GetOpenConnection())
            {
                var lastRecord = (await conn.QueryFirstOrDefaultAsync<StockDto>("Select TOP 1 * From Stocks Order By Id Desc"));
                if (lastRecord == null)
                {
                    _logger.LogTrace("Last record received null");
                    return null;
                }
                return lastRecord;
                //if (lastRecord.Date.Equals(DateTime.Today))
                //{
                //    _logger.LogTrace("Find record for today");
                //    return lastRecord;
                //}
                //else
                //{
                //    _logger.LogTrace("There is not any record for today");
                //    return null;
                //}
            }
        }

        public Task<IEnumerable<StockDto>> GetTodaysRecordsFromStocks()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> InsertToStocks(HashSet<StockDto> list)
        {
            using (SqlConnection conn = GetOpenConnection())
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(conn))
                {
                    copy.DestinationTableName = "Stocks";
                    DataTable table = new DataTable("Stocks");
                    table.Columns.Add("StockName", typeof(string));
                    table.Columns.Add("FinalPrice", typeof(decimal));
                    table.Columns.Add("YesterdayPrice", typeof(decimal));
                    table.Columns.Add("DailyChange", typeof(decimal));
                    table.Columns.Add("HighestPrice", typeof(decimal));
                    table.Columns.Add("LowestPrice", typeof(decimal));
                    table.Columns.Add("AveragePrice", typeof(decimal));
                    table.Columns.Add("VolumeLot", typeof(long));
                    table.Columns.Add("VolumeTL", typeof(long));
                    table.Columns.Add("Date", typeof(DateTime));
                    table.Columns.Add("CreatedAt", typeof(DateTime));

                    foreach (var s in list)
                    {
                        table.Rows.Add(s.StockName, s.FinalPrice, s.YesterdayPrice, s.DailyChange, s.HighestPrice, s.LowestPrice, s.AveragePrice, s.VolumeLot, s.VolumeTL,
                            DateTime.Today, DateTime.UtcNow);
                    }
                    if (table.Rows.Count > 0)
                    {
                        await copy.WriteToServerAsync(table);
                    }
                }
            }
            _logger.LogTrace("Stocks have been inserted to STOCKS table");
            return new ServiceResult(ServiceStatus.Ok);
        }

        Task<List<BistStockDto>> IStockRepository.GetStockByDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
