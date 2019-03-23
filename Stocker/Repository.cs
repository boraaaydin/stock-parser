using Dapper;
using Stocker.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocker
{
    public class Repository
    {
        private string connString;

        public Repository()
        {
            connString = "Server=.;Database=Stocks;Trusted_Connection=True;";
        }
        protected SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }
        public async Task<StockDto> GetLastRecord()
        {
            using (SqlConnection conn = GetOpenConnection())
            {
                var lastRecord= (await conn.QueryAsync<StockDto>("Select * From Stocks Order By Id Desc")).FirstOrDefault();
                return lastRecord;
            }
        }
        public async Task WriteAll(List<StockDto> list)
        {   
            using (SqlConnection conn = GetOpenConnection())
            {
                await InsertRecordsSqlBulkCopy(conn, list);
            }
        }

        async Task InsertRecordsSqlBulkCopy(SqlConnection conn, List<StockDto> list)
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
                table.Columns.Add("VolumeLot", typeof(int));
                table.Columns.Add("VolumeTL", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));
                table.Columns.Add("CreatedAt", typeof(DateTime));

                foreach (var s in list)
                {
                    table.Rows.Add(s.StockName,s.FinalPrice,
                        s.YesterdayPrice,s.DailyChange,s.HighestPrice,
                        s.LowestPrice,s.AveragePrice,s.VolumeLot,s.VolumeTL,
                        DateTime.Today,DateTime.UtcNow);
                }

                await copy.WriteToServerAsync(table);
            }
        }
    }
}
