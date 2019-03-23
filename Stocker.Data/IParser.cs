using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocker.Data
{
    public interface IParser
    {
        Task<List<StockDto>> GetData();
    }
}
