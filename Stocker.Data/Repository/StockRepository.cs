using Dapper;
using Microsoft.Extensions.Logging;
using Stocker.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocker.Data.Repository
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
            _logger.LogTrace("Bugün kayıt yapılıp yapılmadığını kontrol etmek için son kayıt çekiliyor");
            using (SqlConnection conn = GetOpenConnection())
            {
                var lastRecord = (await conn.QueryFirstOrDefaultAsync<StockDto>("Select TOP 1 * From Stocks Order By Id Desc"));
                _logger.LogTrace("Son kayıt çekilde");
                if (lastRecord == null)
                {
                    return null;
                }
                if (lastRecord.Date.Equals(DateTime.Today))
                {
                    _logger.LogTrace("Bugün kayıt çekilmiş");
                }
                else
                {
                    _logger.LogTrace("Bugün kayıt çekilmemiş");
                }
                return lastRecord;
            }
        }

        public async Task AddToStocks(List<StockDto> list)
        {
            _logger.LogTrace("Veritabanına yazılıyor...");
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
            _logger.LogTrace("Veritabanına yazılma işlemi tamamlandı.");
        }

    }
}
