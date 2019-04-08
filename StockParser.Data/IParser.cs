using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockParser.Data
{
    public interface IParser
    {
        Task<List<StockDto>> GetData();
    }
}
