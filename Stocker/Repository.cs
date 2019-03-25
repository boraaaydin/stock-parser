﻿using Dapper;
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
                var lastRecord = (await conn.QueryFirstAsync<StockDto>("Select * From Stocks Order By Id Desc"));
                return lastRecord;
            }
        }

        public async Task WriteAll(List<StockDto> list)
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
                    table.Columns.Add("VolumeLot", typeof(int));
                    table.Columns.Add("VolumeTL", typeof(int));
                    table.Columns.Add("Date", typeof(DateTime));
                    table.Columns.Add("CreatedAt", typeof(DateTime));

                    foreach (var s in list)
                    {
                        table.Rows.Add(s.StockName, s.FinalPrice, s.YesterdayPrice, s.DailyChange, s.HighestPrice, s.LowestPrice, s.AveragePrice, s.VolumeLot, s.VolumeTL,
                            DateTime.Today, DateTime.UtcNow);
                    }

                    await copy.WriteToServerAsync(table);
                }
            }
        }

        public async Task<ServiceResult> InsertToBIST(List<StockDto> list)
        {
            try
            {
                var query = new StringBuilder();
                query.Append("Insert into BIST (");
                query.Append("StockDate,");
                query.Append(String.Join(",", list.Select(x => x.StockName).ToList()));
                query.Append(") Values (");
                query.Append("GETDATE(),");
                query.Append(String.Join(",", list.Select(x => x.FinalPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)).ToList()));
                query.Append(")");
                using (SqlConnection conn = GetOpenConnection())
                {
                    var result=await conn.ExecuteAsync(query.ToString());
                    if (result > 0)
                    {
                        return new ServiceResult(ServiceStatus.Created);
                    }
                    return new ServiceResult(ServiceStatus.NotCreated,"Kayıt yapılmadı");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(ServiceStatus.Error, ex.Message);
            }        
        }

        public ServiceResult AddDecimal62ColumnInDb(string dbName, List<string> columnNames)
        {
            if (columnNames != null)
            {
                if (columnNames.Count > 0)
                {
                    int totalAffectedRows = 0;
                    using (SqlConnection conn = GetOpenConnection())
                    {
                        //using (var tran = conn.BeginTransaction())
                        //{
                        try
                        {
                            foreach (var column in columnNames)
                            {
                                var affectedRow = conn.ExecuteAsync($"ALTER TABLE {dbName} ADD {column} decimal(6,2);");
                                totalAffectedRows += 1;// affectedRow;
                            }
                        }
                        catch (Exception ex)
                        {
                            //tran.Rollback();
                            return new ServiceResult(ServiceStatus.Error, ex.Message);
                        }

                        //}
                    }
                    if (totalAffectedRows != columnNames.Count)
                    {
                        return new ServiceResult(ServiceStatus.Error, "kolon sayısı kadar eklenmedi");
                    }
                }
            }
            return new ServiceResult(ServiceStatus.Created);
        }

        public async Task<List<string>> GetColomnNamesFromDbAsync(string dbName)
        {
            using (SqlConnection conn = GetOpenConnection())
            {
                var result = (await conn.QueryAsync<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{dbName}'")).ToList();
                return result;
            }
        }


    }
}
