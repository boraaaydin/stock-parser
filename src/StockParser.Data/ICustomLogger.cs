using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.Data
{
    public interface ICustomLogger
    {
        void LogInformation(string text);
        void LogDebug(string text);
        void LogError(string text);
    }
}
