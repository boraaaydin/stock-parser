﻿using Dapper;
using Microsoft.Extensions.Logging;
using StockParser.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Data.Repository
{
    public class StockRepository : BaseRepository
    {
        private ILogger<StockRepository> _logger;

        public StockRepository(ILogger<StockRepository> logger)
        {
            _logger = logger;
        }

        public async Task<StockDto> GetLastRecordFromStocks()
        {
            _logger.LogTrace("Getting last record from Stock Table");
            using (SqlConnection conn = GetOpenConnection())
            {
                var lastRecord = (await conn.QueryFirstOrDefaultAsync<StockDto>("Select TOP 1 * From Stocks Order By Id Desc"));
                _logger.LogTrace("Last record received");
                if (lastRecord == null)
                {
                    _logger.LogTrace("Last record received null");
                    return null;
                }
                if (lastRecord.Date.Equals(DateTime.Today))
                {
                    _logger.LogTrace("Find record for today");
                }
                else
                {
                    _logger.LogTrace("There is not any record for today");
                }
                return lastRecord;
            }
        }

        public async Task AddToStocks(List<StockDto> list)
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
        }

    }
}
