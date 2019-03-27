﻿using Dapper;
using Microsoft.Extensions.Logging;
using Stocker.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocker.Data
{
    public class Repository
    {
        private string connString;
        private ILogger<Repository> _logger;

        public Repository(ILogger<Repository> logger)
        {
            connString = "Server=.;Database=Stocks;Trusted_Connection=True;";
            _logger = logger;
        }
        protected SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }

        public async Task<StockDto> GetLastRecordFromStocks()
        {
            _logger.LogTrace("Bugün kayıt yapılıp yapılmadığını kontrol etmek için son kayıt çekiliyor");
            using (SqlConnection conn = GetOpenConnection())
            {
                var lastRecord = (await conn.QueryFirstAsync<StockDto>("Select * From Stocks Order By Id Desc"));
                _logger.LogTrace("Son kayıt çekilde");
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

                    await copy.WriteToServerAsync(table);
                }
            }
            _logger.LogTrace("Veritabanına yazılma işlemi tamamlandı.");
        }

        public async Task<ServiceResult> InsertToBIST(List<StockDto> list)
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
                var result = await conn.ExecuteAsync(query.ToString());
                if (result > 0)
                {
                    return new ServiceResult(ServiceStatus.Created);
                }
                return new ServiceResult(ServiceStatus.NotCreated, "Kayıt yapılmadı");
            }
        }

        /// <summary>
        /// Returns Created if Success
        /// Mesajında oluşturulan hisseleri döner
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public async Task<ServiceResult> AddDecimal62ColumnInDb(string dbName, List<string> columnNames)
        {
            if (columnNames != null)
            {
                if (columnNames.Count > 0)
                {
                    _logger.LogTrace($"{String.Join(",", columnNames)} stok isimleri tabloya ekleniyor");
                    int totalAffectedRows = 0;
                    using (SqlConnection conn = GetOpenConnection())
                    {
                        try
                        {
                            foreach (var column in columnNames)
                            {
                                var affectedRow = await conn.ExecuteAsync($"ALTER TABLE {dbName} ADD {column} decimal(6,2);");
                                totalAffectedRows += 1;// affectedRow;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            return new ServiceResult(ServiceStatus.Error, ex.Message);
                        }
                    }
                    if (totalAffectedRows != columnNames.Count)
                    {
                        var errorMessage = "kolon sayısı kadar eklenmedi";
                        _logger.LogError(errorMessage);
                        return new ServiceResult(ServiceStatus.Error, errorMessage);
                    }

                    _logger.LogTrace("Kayıtlar eklendi");
                    return new ServiceResult(ServiceStatus.Created, string.Join(", ", columnNames));
                }
                else
                {
                    return new ServiceResult(ServiceStatus.NotCreated, "liste boş geldi");
                }
            }
            return new ServiceResult(ServiceStatus.NotCreated, "liste null geldi");
        }

        public async Task<List<string>> GetColumnNamesFromDbAsync(string dbName)
        {
            _logger.LogTrace("Kayıtlı stok isimleri ekleniyor");
            using (SqlConnection conn = GetOpenConnection())
            {
                var result = (await conn.QueryAsync<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{dbName}'")).ToList();
                _logger.LogTrace($"{result.Count} kayıt bulundu.");
                return result;
            }
        }

        public async Task<ServiceResult> AddMissingColoumns(List<StockDto> stocks)
        {
            if(stocks==null)
            {
                return new ServiceResult(ServiceStatus.Error, "Hisseler çekilmedi");
            }
            var presentColoums = GetColumnNamesFromDbAsync("BIST").Result;
            var presentColoumsExceptSome = presentColoums.Except(new List<string> { "Id", "StockDate" }).ToList();
            var colomnNames = stocks.Select(x => x.StockName);
            var newColomns = colomnNames.Except(presentColoumsExceptSome).ToList();
            Console.WriteLine($"{newColomns.Count} adet yeni kolon eklenecek");
            _logger.LogTrace($"{newColomns.Count} adet yeni kolon eklenecek");
            return AddDecimal62ColumnInDb("BIST", newColomns).Result;
        }

    }
}
