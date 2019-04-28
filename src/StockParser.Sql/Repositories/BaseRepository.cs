using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace StockParser.Sql.Repositories
{
    public class BaseRepository
    {
        private SqlContext _context;

        public BaseRepository(SqlContext context)
        {
            _context = context;
        }
        protected SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(_context.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
