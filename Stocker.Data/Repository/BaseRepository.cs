using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Stocker.Data.Repository
{
    public class BaseRepository
    {
        protected SqlConnection GetOpenConnection()
        {
            var connString = "Server=.;Database=Stocks;Trusted_Connection=True;";
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }
    }
}
