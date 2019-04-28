using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.Sql
{
    public class SqlContext
    {
        public string ConnectionString { get; set; }

        public SqlContext(string connString)
        {
            ConnectionString = connString;
        }
    }
}
