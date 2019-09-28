using System;

namespace StockParser.Data
{
    public interface ICustomLogger
    {
        void LogTrace(string text);
        void LogInformation(string text);
        void LogDebug(string text);
        void LogError(string text);
        void LogError(Exception ex, string text);
    }
}
