using Dapper;
using Microsoft.Extensions.Logging;
using StockParser.Common;
using StockParser.Data;
using StockParser.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Sql.Repositories
{
    public class BistRepository : BaseRepository, IBistRepository
    {
        private ILogger<BistRepository> _logger;

        public BistRepository(ILogger<BistRepository> logger, SqlContext context) : base(context)
        {
            _logger = logger;
        }

        public async Task<StockDto> GetTodaysRecordFromStocks()
        {
            _logger.LogTrace("Getting last record from Stock Table");
            using (SqlConnection conn = GetOpenConnection())
            {
                var lastRecord = (await conn.QueryFirstOrDefaultAsync<StockDto>("Select TOP 1 * From Bist Order By Id Desc"));
                _logger.LogTrace("Last record received");
                if (lastRecord == null)
                {
                    _logger.LogTrace("Last record received null");
                    return null;
                }
                if (lastRecord.Date.Equals(DateTime.Today))
                {
                    _logger.LogTrace("Find record for today");
                    return lastRecord;
                }
                else
                {
                    _logger.LogTrace("There is not any record for today");
                    return null;
                }
            }
        }

        public async Task<ServiceResult> InsertToBIST(HashSet<StockDto> list)
        {
            AddMissingColumns(list).Wait();
            var query = new StringBuilder();
            query.Append("Insert into Bist (");
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
                return new ServiceResult(ServiceStatus.NotCreated, "Nothing created");
            }
        }

        private async Task<ServiceResult> AddMissingColumns(HashSet<StockDto> stocks)
        {
            if (stocks == null)
            {
                return new ServiceResult(ServiceStatus.Error, "Stock columns could not added");
            }
            var presentColoums = await GetColumnNamesFromDbAsync();
            var presentColoumsExceptBaseColumns = presentColoums.Except(new List<string> { "Id", "StockDate" }).ToList();
            var newColumns = stocks.Select(x => x.StockName).Except(presentColoumsExceptBaseColumns).ToList();
            _logger.LogTrace($"{newColumns.Count} stock colums will be added");
            return await AddDecimalColumnInDb(newColumns);
        }

        private async Task<List<string>> GetColumnNamesFromDbAsync()
        {
            using (SqlConnection conn = GetOpenConnection())
            {
                var result = (await conn.QueryAsync<string>($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bist'")).ToList();
                return result;
            }
        }

        private async Task<ServiceResult> AddDecimalColumnInDb(List<string> columnNames)
        {
            if (columnNames != null)
            {
                if (columnNames.Count > 0)
                {
                    _logger.LogTrace($"{String.Join(",", columnNames)} stocks will be to Bist table");
                    int totalAffectedRows = 0;
                    using (SqlConnection conn = GetOpenConnection())
                    {
                        try
                        {
                            foreach (var column in columnNames)
                            {
                                var affectedRow = await conn.ExecuteAsync($"ALTER TABLE Bist ADD {column} decimal(8,2);");
                                totalAffectedRows += 1;
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
                        var errorMessage = "Some colums could not be added";
                        _logger.LogError(errorMessage);
                        return new ServiceResult(ServiceStatus.Error, errorMessage);
                    }

                    _logger.LogTrace("Kayıtlar eklendi");
                    return new ServiceResult(ServiceStatus.Created, string.Join(", ", columnNames));
                }
                else
                {
                    return new ServiceResult(ServiceStatus.NotCreated, "Column name list is empty");
                }
            }
            return new ServiceResult(ServiceStatus.NotCreated, "Column name list is null");
        }

    }
}
